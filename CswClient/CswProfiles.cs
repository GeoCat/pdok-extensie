using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace pdok4arcgis {
    /// <summary>
    /// The collection of CSw profiles
    /// </summary>
   public class CswProfiles : CswObjects {

       public static String DEFAULT_CSW_NAMESPACE = "http://www.opengis.net/cat/csw";

        public new CswProfile this[string key] {
            get { return (CswProfile)base[key]; }
        }

        public new CswProfile this[int index] {
            get {
                return (CswProfile)base[index];
            }
        }

        #region "PublicMethods"

        /// <summary>
        /// Add a key value pair to profile collection. 
        /// </summary>
        /// <remarks>
        /// Add to profile collection.
        /// </remarks>
        /// <param name="param1">The key which is the url hashcode for the profile</param>
        /// <param name="param2">The profile object</param>

        public void AddProfile(object key, CswProfile profile) {
            base.Add(key,profile);
        }

        /// <summary>
        /// Loads the profile details from configuration file. 
        /// </summary>
        /// <remarks>
        /// The profiles details are loaded in the collection.
        /// Duplicate or invalid profiles are ignored.
        /// </remarks>
        /// <param name="param2">the profile configuration file</param>
        public void loadProfilefromConfig(string filename) {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                XmlNodeList xmlnodes = doc.GetElementsByTagName("Profile");

                foreach (XmlNode xmlnode in xmlnodes) {
                    string cswnamespace = "";
                    XmlNode cswNamespaceNode = xmlnode.SelectSingleNode("CswNamespace");
                    if (cswNamespaceNode != null) cswnamespace = cswNamespaceNode.InnerText.Trim();
                    if (cswnamespace.Length <= 0) {
                        cswnamespace =CswProfiles.DEFAULT_CSW_NAMESPACE ;
                    }
                    string id = xmlnode.SelectSingleNode("ID").InnerText.Trim();
                    string name = xmlnode.SelectSingleNode("Name").InnerText.Trim();                    
                    string description = xmlnode.SelectSingleNode("Description").InnerText.Trim();
                    string requestxslt = xmlnode.SelectSingleNode("GetRecords/XSLTransformations/Request").InnerText.Trim();
                    string responsexslt = xmlnode.SelectSingleNode("GetRecords/XSLTransformations/Response").InnerText.Trim();                   
                    string requestKVPs = xmlnode.SelectSingleNode("GetRecordByID/RequestKVPs").InnerText.Trim();
                    bool livedatamap = bool.Parse(xmlnode.SelectSingleNode("SupportContentTypeQuery").InnerText.Trim());
                    bool extentsearch = bool.Parse(xmlnode.SelectSingleNode("SupportSpatialQuery").InnerText.Trim());
                    bool spatialboundary = bool.Parse(xmlnode.SelectSingleNode("SupportSpatialBoundary").InnerText.Trim());
                    string metadataxslt = "";
                    XmlNode node = xmlnode.SelectSingleNode("GetRecordByID/XSLTransformations/Response");
                    if (node != null) { metadataxslt = node.InnerText.Trim(); }

                    string displayResponseXslt = "";
                    node = xmlnode.SelectSingleNode("GetRecordByID/XSLTransformations/StyleResponse");
                    if (node != null) { displayResponseXslt = node.InnerText.Trim(); }

                    // generate full path for XSLT files
                    // handles both absolute path and relative path, assuming profile xml is the root for relative path
                    string profileDirName = System.IO.Path.GetDirectoryName (filename);
                    requestxslt = System.IO.Path.Combine(profileDirName, requestxslt);
                    responsexslt = System.IO.Path.Combine(profileDirName, responsexslt);
                    if (metadataxslt != "") { metadataxslt = System.IO.Path.Combine(profileDirName, metadataxslt); }

                    if (displayResponseXslt != "") { displayResponseXslt = System.IO.Path.Combine(profileDirName, displayResponseXslt); }

                    this.AddProfile(id, new CswProfile(id, name, cswnamespace, description, requestKVPs, requestxslt, responsexslt, metadataxslt, displayResponseXslt,livedatamap, extentsearch, spatialboundary));
                }

            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
                throw ex;
            }

        }

        #endregion
    }
}

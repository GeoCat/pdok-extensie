using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;


namespace pdok4arcgis { /// <summary>
    /// Maintains information of CSW Catalog
    /// </summary>
    /// <remarks>
    /// The catalogs contain all the information like url, profile information
    /// credentials and capabilities.
    /// </remarks>
    /// 
    public class CswCatalog:IComparable
    {
        private static int IDCounter = 1 ;
        private string id;
        private string name;
        private bool locking;
        private string url;
        private string baseurl;
        private CswProfile profile;
        private UsernamePasswordCredentials credentials;
        private CswCatalogCapabilities capabilities;
        private bool isConnect;

        #region "Properties"

        public string Name {
            set {
                if (value.Length > 0)
                    name = value;
                else
                    name = url;
            }
            get {
                return name;
            }
            
        }

        public string URL {
            set {
                url = value;
            }
            get {
                return url;
            }
        }

        public string BaseURL {
            set {
                baseurl = value;
            }
            get {
                return baseurl;
            }
        }

        public CswProfile Profile {
            set {
                profile = value;
            }
            get {
                return profile;
            }
        }

        public UsernamePasswordCredentials Credentials {
            set {
                credentials = value;
            }
            get {
                return credentials;
            }
        }

        public int HashKey {
            get {
                return url.GetHashCode();
            }
        }

        public string ID {
            get {
                return id;
            }
        }


        public bool Locking {
            set {
                locking = value;
            }
            get {
                return locking;
            }

        }


        internal CswCatalogCapabilities Capabilities
        {
            get
            {
                return capabilities;
            }
        }


    #endregion


        public int CompareTo(object obj) {
            if (obj is CswCatalog) {
                CswCatalog catalog = (CswCatalog)obj;
                return this.Name.CompareTo(catalog.Name);
            }
            throw new ArgumentException("object is not a CswCatalog");
        }


    # region constructor definition

        public CswCatalog() {
            id = "catalog"+ IDCounter.ToString();
            IDCounter++;
        }

        public CswCatalog(String surl, String sname, CswProfile oprofile) {
            id = "catalog" + IDCounter.ToString();
            IDCounter++;
            URL = surl;
            Profile = oprofile;
            Name = sname;
            Locking = false;
                    
        }

    #endregion

    #region "PrivateFunction"

        /// <summary>
        ///  To retrieve informations about the CSW service.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="param1">capabilities baseurl</param>
        /// <returns>Response the get capabilities url</returns>
        private string GetCapabilities(string capabilitiesurl) {
            try {
                CswClient client = new CswClient();
                string response = client.SubmitHttpRequest("GET", capabilitiesurl, "");
                //Console.WriteLine()
                

                XmlDocument xmlDoc = new XmlDocument();                               
                xmlDoc.LoadXml(response); 
                XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(xmlDoc.NameTable);
                if (this.Profile.CswNamespace.Length <= 0) {
                    this.Profile.CswNamespace = CswProfiles.DEFAULT_CSW_NAMESPACE;
                }
                xmlnsManager.AddNamespace("ows", "http://www.opengis.net/ows");
                xmlnsManager.AddNamespace("csw", this.Profile.CswNamespace);
                xmlnsManager.AddNamespace("wrs10", "http://www.opengis.net/cat/wrs/1.0");
                xmlnsManager.AddNamespace("wrs", "http://www.opengis.net/cat/wrs");
                xmlnsManager.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
                xmlnsManager.AddNamespace("wcs", "http://www.opengis.net/wcs");
                if (xmlDoc.SelectSingleNode("/csw:Capabilities|/wrs:Capabilities| /wrs10:Capabilities | /wcs:WCS_Capabilities", xmlnsManager) != null)
                    return response;
                else
                    return null;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        ///  To retrieve informations about the CSW service.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="param1">capabilitiesxml details </param>
        private bool ParseCapabilities(string capabilitiesxml) {
            try {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(capabilitiesxml);
                XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(xmlDoc.NameTable);
                if (this.Profile.CswNamespace.Length <= 0) {
                    this.Profile.CswNamespace = CswProfiles.DEFAULT_CSW_NAMESPACE;
                }
                xmlnsManager.AddNamespace("ows", "http://www.opengis.net/ows");
                xmlnsManager.AddNamespace("csw", this.Profile.CswNamespace);
                xmlnsManager.AddNamespace("wrs", "http://www.opengis.net/cat/wrs");
                xmlnsManager.AddNamespace("wrs10", "http://www.opengis.net/cat/wrs/1.0");
                xmlnsManager.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
                xmlnsManager.AddNamespace("wcs", "http://www.opengis.net/wcs");
                //setting capabilities
                if (capabilities == null) capabilities = new CswCatalogCapabilities();

                if (isWCSService(capabilitiesxml))
                {
                    capabilities.GetRecordByID_GetURL = xmlDoc.SelectSingleNode("/wcs:WCS_Capabilities/wcs:Capability/wcs:Request/wcs:GetCoverage/wcs:DCPType/wcs:HTTP/wcs:Get/wcs:OnlineResource", xmlnsManager).Attributes["xlink:href"].Value;

                    if (capabilities.GetRecordByID_GetURL == null || capabilities.GetRecordByID_GetURL.Trim().Length == 0)
                    {
                        throw new Exception("No valid Get Coverage Url for Csw Client");
                    }
                }
                else
                {

                 //   XmlNodeList parentNodeList = xmlDoc.SelectNodes("//ows:OperationsMetadata/ows:Operation[@name='GetRecords']/ows:DCP/ows:HTTP/ows:Post", xmlnsManager);

                    capabilities.GetRecords_PostURL = xmlDoc.SelectSingleNode("//ows:OperationsMetadata/ows:Operation[@name='GetRecords']/ows:DCP/ows:HTTP/ows:Post", xmlnsManager).Attributes["xlink:href"].Value;

                  /*  if (parentNodeList.Count > 1)
                    {
                        XmlNodeList nodeList = xmlDoc.SelectNodes("//ows:OperationsMetadata/ows:Operation[@name='GetRecords']/ows:DCP/ows:HTTP/ows:Post/ows:Constraint/ows:Value", xmlnsManager);
                        if (nodeList != null && nodeList.Count >0 && (nodeList.Item(0).InnerText.Equals("SOAP") || nodeList.Item(0).InnerText.Equals("soap")))
                        {
                            capabilities.GetRecords_IsSoapEndPoint = true;
                        }
                        else if(capabilities.GetRecords_PostURL.ToLower().Contains("soap"))
                        {
                            capabilities.GetRecords_IsSoapEndPoint = true;
                        }
                        //  capabilities.GetRecords_PostURL = xmlDoc.SelectSingleNode("//ows:OperationsMetadata/ows:Operation[@name='GetRecords']/ows:DCP/ows:HTTP/ows:Post", xmlnsManager).Attributes["xlink:href"].Value;
                    }*/
                    
                   
                  
                   XmlNodeList nodeList = xmlDoc.SelectNodes("//ows:OperationsMetadata/ows:Operation[@name='GetRecords']/ows:DCP/ows:HTTP/ows:Post/ows:Constraint/ows:Value", xmlnsManager);

                   for (int iter = 0; iter < nodeList.Count; iter++)
                    {
                        capabilities.GetRecords_PostURL = nodeList.Item(iter).ParentNode.ParentNode.Attributes["xlink:href"].Value;

                        if (this.profile.Name == "INSPIRE CSW 2.0.2 AP ISO")
                        {
                            if (nodeList.Item(iter).InnerText.Equals("SOAP") || nodeList.Item(iter).InnerText.Equals("soap") || capabilities.GetRecords_PostURL.ToLower().Contains("soap"))
                            {                                
                                capabilities.GetRecords_IsSoapEndPoint = true;
                                break;
                            }
                        }
                        else
                        {
                            if (nodeList.Item(iter).InnerText.Equals("XML") || nodeList.Item(iter).InnerText.Equals("xml"))
                            {
                                 break;
                            }
                        }
                    }
                    


                    if (capabilities.GetRecords_PostURL == null || capabilities.GetRecords_PostURL.Trim().Length == 0)
                    {
                        throw new Exception("No valid POST Url for Csw Client");
                    }

                    //   capabilities.GetRecords_PostURL = xmlDoc.SelectSingleNode("//ows:OperationsMetadata/ows:Operation[@name='GetRecords']/ows:DCP/ows:HTTP/ows:Post", xmlnsManager).Attributes["xlink:href"].Value;

                    capabilities.GetRecordByID_GetURL = xmlDoc.SelectSingleNode("//ows:OperationsMetadata/ows:Operation[@name='GetRecordById']/ows:DCP/ows:HTTP/ows:Get", xmlnsManager).Attributes["xlink:href"].Value;
                }

                return true;
            }
            catch (Exception e) {
                return false;
            }
            
        }

        private Boolean isWCSService(String capabilitiesXml)
        {
            return capabilitiesXml.IndexOf("WCS_Capabilities") > -1 ? true : false;

        }

    #endregion

    #region "PublicFunction"

        /// <summary>
        ///  To connect to a catalog service.
        ///  The capabilties details are populated based on the service. 
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>true if connection can be made to the csw service</returns>
        public bool Connect(bool isWriteLogs) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(DateTime.Now + " Sending GetCapabilities request to URL : " + URL);
            string capabilitesxml = GetCapabilities(URL);            
            if (capabilitesxml != null){
                sb.AppendLine(DateTime.Now + " GetCapabilitiesResponse xml : " + capabilitesxml);
                ParseCapabilities(capabilitesxml);
                sb.AppendLine(DateTime.Now + " Parsed GetCapabilitiesResponse xml...");                
                isConnect = true;
                if (isWriteLogs)
                    Utils.writeFile(sb.ToString());
                return true;
            }
            else {
                sb.AppendLine(DateTime.Now + " Failed to connect to GetCapabilities endpoint.");
                if (isWriteLogs)
                    Utils.writeFile(sb.ToString());
                isConnect = false;
                return false;
            }           
        }

        /// <summary>
        ///  To test if already connected to a catalog service.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>true if connection has already been made to the csw service else false</returns>
        public bool IsConnected() {        
               return isConnect;
       
        }

        public void resetConnection()
        {
            isConnect = false;

        }

    #endregion
    }
}

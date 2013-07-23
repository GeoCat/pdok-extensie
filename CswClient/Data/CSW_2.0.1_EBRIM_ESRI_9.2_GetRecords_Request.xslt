<?xml version="1.0" encoding="UTF-8"?>
<!-- Search keyword in Description on ebRim (ArcIMS 92 CSW connector)-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" indent="no" encoding="UTF-8" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <xsl:element name="csw:GetRecords" use-attribute-sets="GetRecordsAttributes" 
                 xmlns:csw="http://www.opengis.net/cat/csw"
                 xmlns:ogc="http://www.opengis.net/ogc"
                 xmlns:gml="http://www.opengis.net/gml" 
                 xmlns:rim="urn:oasis:names:tc:ebxml-regrep:xsd:rim:3.0"
                 >
      <csw:Query typeNames="rim:RegistryObject">
        <csw:ElementSetName>summary</csw:ElementSetName>
        <csw:Constraint version="1.0.0">
          <ogc:Filter>
            <ogc:PropertyIsLike>
              <ogc:PropertyName>/rim:ExtrinsicObject/rim:Description/rim:LocalizedString/@value</ogc:PropertyName>
              <ogc:Literal>
                <xsl:value-of select="/GetRecords/KeyWord"/>
              </ogc:Literal>
            </ogc:PropertyIsLike>
        </ogc:Filter>
      </csw:Constraint>
    </csw:Query>
  </xsl:element>

  </xsl:template>
  <xsl:attribute-set name="GetRecordsAttributes" 
                     xmlns:csw="http://www.opengis.net/cat/csw" 
                     xmlns:ogc="http://www.opengis.net/ogc"
                     xmlns:gml="http://www.opengis.net/gml" 
                     xmlns:rim="urn:oasis:names:tc:ebxml-regrep:xsd:rim:3.0">
    <xsl:attribute name="outputFormat">application/xml</xsl:attribute>
    <xsl:attribute name="outputSchema">urn:oasis:names:tc:ebxml-regrep:xsd:rim:3.0</xsl:attribute>
    <xsl:attribute name="version">2.0.1</xsl:attribute>
    <xsl:attribute name="service">CSW</xsl:attribute>
    <xsl:attribute name="startPosition"><xsl:value-of select="/GetRecords/StartPosition"/></xsl:attribute>
    <xsl:attribute name="maxRecords"><xsl:value-of select="/GetRecords/MaxRecords"/></xsl:attribute>
  </xsl:attribute-set>
</xsl:stylesheet>

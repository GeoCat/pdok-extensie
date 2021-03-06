<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:csw="http://www.opengis.net/cat/csw/2.0.2" xmlns:ows="http://www.opengis.net/ows" xmlns:dct="http://purl.org/dc/terms/" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:dcmiBox="http://dublincore.org/documents/2000/07/11/dcmi-box/" xmlns:gmd="http://www.isotc211.org/2005/gmd" xmlns:gco="http://www.isotc211.org/2005/gco" xmlns:gml="http://www.opengis.net/gml"  xmlns:srv="http://www.isotc211.org/2005/srv">
	<xsl:output method="xml" indent="yes" encoding="UTF-8" omit-xml-declaration="no"/>
	<xsl:template match="/">
		<Records>
            <xsl:attribute name="numberOfRecordsMatched">
                <xsl:value-of
                          select="/csw:GetRecordsResponse/csw:SearchResults/@numberOfRecordsMatched" />
            </xsl:attribute>
			<xsl:for-each select="/csw:GetRecordsResponse/csw:SearchResults/gmd:MD_Metadata">
				<Record>
					<ID>
						<xsl:value-of select="gmd:fileIdentifier/gco:CharacterString"/>
					</ID>
                    <ReponsibleOrganisation>
                        <xsl:value-of select="gmd:contact/gmd:CI_ResponsibleParty/gmd:organisationName/gco:CharacterString"/>
                    </ReponsibleOrganisation>
					<Title>
						<xsl:value-of select="gmd:identificationInfo/gmd:MD_DataIdentification/gmd:citation/gmd:CI_Citation/gmd:title/gco:CharacterString|gmd:identificationInfo/srv:SV_ServiceIdentification/gmd:citation/gmd:CI_Citation/gmd:title/gco:CharacterString"/>
					</Title>
					<Abstract>
						<xsl:value-of select="gmd:identificationInfo/gmd:MD_DataIdentification/gmd:abstract/gco:CharacterString|gmd:identificationInfo/srv:SV_ServiceIdentification/gmd:abstract/gco:CharacterString"/>
					</Abstract>
                    <MetadataVersion>
                        <xsl:value-of select="gmd:metadataStandardVersion/gco:CharacterString"/>
                    </MetadataVersion>
					<LowerCorner>
						<xsl:value-of
							select="./gmd:identificationInfo/gmd:MD_DataIdentification/gmd:extent/gmd:EX_Extent/gmd:geographicElement/gmd:EX_GeographicBoundingBox/gmd:westBoundLongitude/gco:Decimal|./gmd:identificationInfo/srv:SV_ServiceIdentification/srv:extent/gmd:EX_Extent/gmd:geographicElement/gmd:EX_GeographicBoundingBox/gmd:westBoundLongitude/gco:Decimal" />
						<xsl:value-of select="' '" />
						<xsl:value-of
							select="./gmd:identificationInfo/gmd:MD_DataIdentification/gmd:extent/gmd:EX_Extent/gmd:geographicElement/gmd:EX_GeographicBoundingBox/gmd:southBoundLatitude/gco:Decimal|./gmd:identificationInfo/srv:SV_ServiceIdentification/srv:extent/gmd:EX_Extent/gmd:geographicElement/gmd:EX_GeographicBoundingBox/gmd:southBoundLatitude/gco:Decimal" />
					</LowerCorner>
					<UpperCorner>
						<xsl:value-of
							select="./gmd:identificationInfo/gmd:MD_DataIdentification/gmd:extent/gmd:EX_Extent/gmd:geographicElement/gmd:EX_GeographicBoundingBox/gmd:eastBoundLongitude/gco:Decimal|./gmd:identificationInfo/srv:SV_ServiceIdentification/srv:extent/gmd:EX_Extent/gmd:geographicElement/gmd:EX_GeographicBoundingBox/gmd:eastBoundLongitude/gco:Decimal" />
						<xsl:value-of select="' '" />
						<xsl:value-of
							select="./gmd:identificationInfo/gmd:MD_DataIdentification/gmd:extent/gmd:EX_Extent/gmd:geographicElement/gmd:EX_GeographicBoundingBox/gmd:northBoundLatitude/gco:Decimal|./gmd:identificationInfo/srv:SV_ServiceIdentification/srv:extent/gmd:EX_Extent/gmd:geographicElement/gmd:EX_GeographicBoundingBox/gmd:northBoundLatitude/gco:Decimal" />
					</UpperCorner>
					
					<!--<xsl:if test="count(./gmd:distributionInfo/gmd:MD_Distribution/gmd:transferOptions/gmd:MD_DigitalTransferOptions/gmd:onLine/gmd:CI_OnlineResource[gmd:protocol/gco:CharacterString='OGC:WMS']/gmd:linkage/gmd:URL) + count(./gmd:identificationInfo/srv:SV_ServiceIdentification/srv:containsOperations/srv:SV_OperationMetadata[srv:operationName/gco:CharacterString='GetCapabilities'])>0">-->
					
              <References>
				  <xsl:choose>
				  <xsl:when test="count(./gmd:identificationInfo/srv:SV_ServiceIdentification/srv:containsOperations/srv:SV_OperationMetadata[srv:operationName/gco:CharacterString='GetCapabilities']/srv:connectPoint/gmd:CI_OnlineResource/gmd:linkage/gmd:URL)>0">
				  <xsl:value-of select="./gmd:identificationInfo/srv:SV_ServiceIdentification/srv:containsOperations/srv:SV_OperationMetadata[srv:operationName/gco:CharacterString='GetCapabilities']/srv:connectPoint/gmd:CI_OnlineResource/gmd:linkage/gmd:URL" /><xsl:text>&#x2714;</xsl:text>urn:x-esri:specification:ServiceType:ArcIMS:Metadata:Server<xsl:text>&#x2715;</xsl:text>
				  <!--<xsl:variable name="sUrlUpper" select="translate($sUrl,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />-->
                    <!--<xsl:if test="not(contains($sUrlUpper,'?'))"><xsl:text>?</xsl:text></xsl:if>
                    <xsl:if test="not(contains($sUrlUpper,'=GETCAPABILITIES'))"><xsl:text>&amp;request=GetCapabilities</xsl:text></xsl:if>
                    <xsl:if test="not(contains($sUrlUpper,'=WMS'))"><xsl:text>&amp;service=WMS</xsl:text></xsl:if>
                    <xsl:if test="not(contains($sUrlUpper,'VERSION='))"><xsl:text>&amp;version=1.3.0</xsl:text></xsl:if>-->
				  </xsl:when>
                  <xsl:when test="count(gmd:distributionInfo/gmd:MD_Distribution/gmd:transferOptions/gmd:MD_DigitalTransferOptions/gmd:onLine/gmd:CI_OnlineResource/gmd:linkage/gmd:URL)>0">
				  <!--use the properties of the first data item, if the service properties are not provided-->
                  <xsl:value-of select="gmd:distributionInfo/gmd:MD_Distribution/gmd:transferOptions/gmd:MD_DigitalTransferOptions/gmd:onLine/gmd:CI_OnlineResource/gmd:linkage/gmd:URL"/><xsl:text>&#x2714;</xsl:text>urn:x-esri:specification:ServiceType:ArcIMS:Metadata:Server<xsl:text>&#x2715;</xsl:text>
                  </xsl:when>
				  </xsl:choose>
                  <xsl:choose>
                      <xsl:when test="count(./gmd:identificationInfo/srv:SV_ServiceIdentification/srv:containsOperations/srv:SV_OperationMetadata[srv:operationName/gco:CharacterString='GetCapabilities']/srv:connectPoint/gmd:CI_OnlineResource/gmd:protocol/gco:CharacterString)>0">
                          <xsl:value-of select="./gmd:identificationInfo/srv:SV_ServiceIdentification/srv:containsOperations/srv:SV_OperationMetadata[srv:operationName/gco:CharacterString='GetCapabilities']/srv:connectPoint/gmd:CI_OnlineResource/gmd:protocol/gco:CharacterString" /><xsl:text>&#x2714;</xsl:text>urn:x-esri:specification:ServiceType:ArcIMS:Metadata:ServiceType<xsl:text>&#x2715;</xsl:text>
                      </xsl:when>
                      <xsl:when test="count(gmd:distributionInfo/gmd:MD_Distribution/gmd:transferOptions/gmd:MD_DigitalTransferOptions/gmd:onLine/gmd:CI_OnlineResource/gmd:protocol/gco:CharacterString)>0">
                          <!--use the properties of the first data item, if the service properties are not provided-->
                            <xsl:value-of select="gmd:distributionInfo/gmd:MD_Distribution/gmd:transferOptions/gmd:MD_DigitalTransferOptions/gmd:onLine/gmd:CI_OnlineResource/gmd:protocol/gco:CharacterString" /><xsl:text>&#x2714;</xsl:text>urn:x-esri:specification:ServiceType:ArcIMS:Metadata:ServiceType<xsl:text>&#x2715;</xsl:text>
                      </xsl:when>
                  </xsl:choose>
              </References>
						<Types>liveData<xsl:text>&#x2714;</xsl:text>urn:x-esri:specification:ServiceType:ArcIMS:Metadata:ContentType<xsl:text>&#x2715;</xsl:text></Types>
						<Type>liveData</Type>
					<!--</xsl:if>-->
				</Record>
			</xsl:for-each>
		</Records>
	</xsl:template>
</xsl:stylesheet>
using System.Xml.XPath;

namespace DocxTemplater.DataSources
{
    public interface IXmlDataSource
    {
        XPathNavigator GetNodeNavigator(string xpath);
        XPathNodeIterator GetNodesIterator(string xpath);
    }
}
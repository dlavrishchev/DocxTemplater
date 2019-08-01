<h1 align="center">
DocxTemplater
</h1>

Fast library written in C# for generating .docx files from a template and xml datasets.

- Based on MS Office Word content control feature and XPath expressions.

- Without external dependencies.

- .NET Standard 2.0.

### Features
- Filling tables and fields.

### Usage

```csharp
using DocxTemplater;

static class Program
{
    static void Main(string[] args)
    {
        var dataset = GenerateDataset(rowsCount:1000);
        using (var doc = new WordDocument(@".\files\template.docx"))
        {
	    doc.ProcessAllContentControls(dataset);
	    doc.Save(@".\files\result.docx");
        }
    }

    private static IXPathNavigable GenerateDataset(int rowsCount)
    {
        var items = new XElement[rowsCount];
        for (var i = 0; i < rowsCount; i++)
        {
            var item = new XElement("item",
                new XElement("c1", i, new XAttribute("textColor", "#FF0000")),
                new XElement("c2", i, new XAttribute("highlightColor", "green")),
                new XElement("c3", i, new XAttribute("bold", "true")),
                new XElement("c4", i, new XAttribute("italic", "true")),
                new XElement("c5", i, new XAttribute("underline", "true")),
                new XElement("c6", i),
                new XElement("c7", i),
                new XElement("c8", i),
                new XElement("c9", i),
                new XElement("c10", i));
            items[i] = item;
        }

        using (var ms = new MemoryStream())
        {
            new XElement("items", items).Save(ms);
            ms.Position = 0;
            return new XPathDocument(ms);
        }
    }
}
```

### Text styling  XML attributes

| Attribute   | Accepted value   |
|:-----------:|:----------------:|
|bold		  |true / false		 |
|italic		  |true / false		 |
|underline	  |true / false		 |
|textColor	  |Color hex code in format '#RRGGBB'. For example, #005ce6	 |
|highlightColor	  |black<br>blue<br>darkBlue<br>cyan<br>darkCyan<br>magenta<br>darkMagenta<br>red<br>darkRed<br>white<br>yellow<br>darkYellow<br>green<br>darkGreen<br>darkGray<br>lightGray|

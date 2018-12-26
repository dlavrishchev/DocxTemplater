<h1 align="center">
DocxTemplater
</h1>

Lightweight library for generating .docx files from a template and xml data.

- Based on Word content control features and XPath expressions.

- Without external dependencies.

- .NET Standard 2.0.

### Usage

```csharp
using DocxTemplater;
using DocxTemplater.DataSources;

var dataSource = new XmlDataSource();
dataSource.Load(@".\files\data.xml");

using (var tp = new TemplateProcessor(@".\files\template.docx"))
{
	tp.Process(dataSource);
	tp.Save(@".\files\result.docx");
}
```
[template.docx](tests/DocxTemplater.Demo/demo_files/template.docx)<br>
[result.docx](tests/DocxTemplater.Demo/demo_files/result.docx)<br>
[data.xml](tests/DocxTemplater.Demo/demo_files/data.xml)

### Text styling  XML attributes

| Attribute   | Accepted value   |
|:-----------:|:----------------:|
|bold		  |true / false		 |
|italic		  |true / false		 |
|underline	  |true / false		 |
|textColor	  |Color hex code in format '#RRGGBB'. For example, #005ce6	 |
|textHighlightColor	  |black<br>blue<br>darkBlue<br>cyan<br>darkCyan<br>magenta<br>darkMagenta<br>red<br>darkRed<br>white<br>yellow<br>darkYellow<br>green<br>darkGreen<br>darkGray<br>lightGray|

```xml
<album>
    <name textColor="#005ce6" bold="true">Wish You Were Here</name>
    <label>Harvest</label>
    <year textHighlightColor="red">1975</year>
</album>
```
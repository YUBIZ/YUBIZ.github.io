namespace Models;

using System.Xml.Serialization;

[XmlType("url", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
public readonly record struct Url([property: XmlElement("loc", Order = 0)] string Location, [property: XmlElement("lastmod", Order = 1)] string LastModified, [property: XmlElement("changefreq", Order = 2)] string? ChangeFrequency, [property: XmlElement("priority", Order = 3)] string? Priority);

[XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
public readonly record struct UrlSet([property: XmlElement("url")] Url[] Urls);
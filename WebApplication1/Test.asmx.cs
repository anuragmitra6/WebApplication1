using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Linq;
using System.IO;
using System.Data;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace WebApplication1
{
    /// <summary>
    /// Summary description for Test
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Test : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }


        //public static XmlDocument ConvertToXmlDocument(XDocument input)
        //{
        //    var xmlDocumentObj = new XmlDocument();
        //    using (var xmlReader = input.CreateReader())
        //    {
        //        xmlDocumentObj.Load(xmlReader);
        //        return xmlDocumentObj;
        //    }

        //}

        [WebMethod(EnableSession = true)]
        public void GetSearchList(string Test, string start)
        {
            var xmlDocument = new XmlDocument();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            XmlDocument doc1 = new XmlDocument();
            string url = "https://eutils.ncbi.nlm.nih.gov/entrez/eutils/esearch.fcgi?db=pubmed&term=" + Test + "&retmax=10&usehistory=y&retstart=" + start;

            DataTable dt = new DataTable();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                XDocument xdoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
                XmlDocument newXmlDocument = ConvertToXmlDocument(xdoc);
                string jsonText = JsonConvert.SerializeXmlNode(newXmlDocument);
                XmlNodeList webenvList = (newXmlDocument.SelectNodes("eSearchResult/WebEnv"));
                XmlNodeList qrylistList = (newXmlDocument.SelectNodes("eSearchResult/QueryKey"));
                XmlNodeList retmaxList = (newXmlDocument.SelectNodes("eSearchResult/RetMax"));
                XmlNodeList retstartList = (newXmlDocument.SelectNodes("eSearchResult/RetStart"));

                string webenv = webenvList[0].InnerText.ToString();
                //HttpContext.Current.Session["webenv"] = webenv;
                string qrylist = qrylistList[0].InnerText.ToString();
                string retmax = retmaxList[0].InnerText.ToString();
                string retstart = retstartList[0].InnerText.ToString();
                int next = Convert.ToInt32(retstart) + Convert.ToInt32(retmax);
                int prev = 1;
                if (Convert.ToInt32(retstart) > Convert.ToInt32(retmax))
                {
                    prev = Convert.ToInt32(retstart) - Convert.ToInt32(retmax);
                }

                string url1 = "https://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db=pubmed&query_key=" + qrylist + "&WebEnv=" + webenv + "&retstart=" + retstart + "&retmax=" + retmax + "&retmode=xml&rettype=abstract";
                HttpResponseMessage response1 = client.GetAsync(url1).Result;
                if (response1.StatusCode == HttpStatusCode.OK)
                {
                    XDocument xdoc1 = XDocument.Parse(response1.Content.ReadAsStringAsync().Result);
                    XmlDocument newXmlDocument1 = ConvertToXmlDocument(xdoc1);

                    XmlNodeList ArticleList = (newXmlDocument1.SelectNodes("PubmedArticleSet/PubmedArticle/MedlineCitation"));
                    dt = ProcessNodes(ArticleList, webenv, qrylist, retmax, next, prev, Test);
                }
            }

            Context.Response.Write(JsonConvert.SerializeObject(dt));
        }

        public static XmlDocument ConvertToXmlDocument(XDocument input)
        {
            var xmlDocumentObj = new XmlDocument();
            using (var xmlReader = input.CreateReader())
            {
                xmlDocumentObj.Load(xmlReader);
                return xmlDocumentObj;
            }

        }

        public static DataTable ProcessNodes(XmlNodeList nodes, string webenv, string qrylist, string max, int total, int prev, string Term)
        {
            DataTable dtval = new DataTable();
            string keyword = "";
            dtval.Columns.Add(new DataColumn("PMID", typeof(String)));
            dtval.Columns.Add(new DataColumn("Title", typeof(String)));
            dtval.Columns.Add(new DataColumn("ArticleTitle", typeof(String)));
            //dtval.Columns.Add(new DataColumn("Name", typeof(String)));
            dtval.Columns.Add(new DataColumn("Keyword", typeof(String)));
            dtval.Columns.Add(new DataColumn("ISOAbbreviation", typeof(String)));
            dtval.Columns.Add(new DataColumn("WebEnv", typeof(String)));
            dtval.Columns.Add(new DataColumn("QryList", typeof(String)));
            dtval.Columns.Add(new DataColumn("RetPrev", typeof(String)));
            dtval.Columns.Add(new DataColumn("RetNext", typeof(String)));
            dtval.Columns.Add(new DataColumn("Term", typeof(String)));

            for (int i = 0; i < nodes.Count; i++)
            {
                keyword = "";
                DataRow dr = dtval.NewRow();
                dtval.Rows.Add(dr);

                dtval.Rows[i]["WebEnv"] = webenv;
                dtval.Rows[i]["QryList"] = qrylist;
                dtval.Rows[i]["RetNext"] = total;
                dtval.Rows[i]["RetPrev"] = prev;
                dtval.Rows[i]["Term"] = Term;

                for (int k = 0; k < nodes[i].ChildNodes.Count; k++)
                {
                    if (nodes[i].ChildNodes[k].Name == "PMID")
                    {
                        dtval.Rows[i]["PMID"] = nodes[i].ChildNodes[k].InnerText;
                    }

                    if (nodes[i].ChildNodes[k].Name == "Article")
                    {
                        for (int m = 0; m < nodes[i].ChildNodes[k].ChildNodes.Count; m++)
                        {
                            if (nodes[i].ChildNodes[k].ChildNodes[m].Name == "Journal")
                            {
                                for (int n = 0; n < nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes.Count; n++)
                                {
                                    if (nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].Name == "Title")
                                    {
                                        dtval.Rows[i]["Title"] = nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].InnerText;
                                    }


                                    if (nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].Name == "ISOAbbreviation")
                                    {
                                        dtval.Rows[i]["ISOAbbreviation"] = nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].InnerText;
                                    }
                                }
                            }

                            if (nodes[i].ChildNodes[k].ChildNodes[m].Name == "ArticleTitle")
                            {
                                dtval.Rows[i]["ArticleTitle"] = nodes[i].ChildNodes[k].ChildNodes[m].InnerText.ToString();
                            }

                            //if (nodes[i].ChildNodes[k].ChildNodes[m].Name == "Abstract")
                            //{
                            //    dtval.Rows[i]["Name"] = nodes[i].ChildNodes[k].ChildNodes[m].InnerText.ToString();
                            //}
                        }
                    }

                    if (nodes[i].ChildNodes[k].Name == "KeywordList")
                    {
                        int count = 0;
                        if (nodes[i].ChildNodes[k].ChildNodes.Count > 3)
                        {
                            count = 3;
                        }
                        else
                        {
                            count = nodes[i].ChildNodes[k].ChildNodes.Count;
                        }

                        for (int j = 0; j < count; j++)
                        {
                            if (keyword == "")
                            {
                                keyword = nodes[i].ChildNodes[k].ChildNodes[j].InnerText;
                            }
                            else
                            {
                                keyword += keyword + " , " + nodes[i].ChildNodes[k].ChildNodes[j].InnerText;
                            }
                        }
                    }

                    dtval.Rows[i]["Keyword"] = keyword;
                }
            }

            return dtval;
        }

        [WebMethod(EnableSession = true)]
        public void GetDetails(string PMID, string WebEnv, string QryList)
        {
            DataTable dt = new DataTable();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            string webenv = WebEnv.ToString();
            //HttpContext.Current.Session["webenv"] = webenv;
            string qrylist = QryList.ToString();

            string url1 = "https://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db=pubmed&query_key=" + qrylist + "&WebEnv=" + WebEnv + "&id=" + PMID + "";
            HttpResponseMessage response1 = client.GetAsync(url1).Result;
            if (response1.StatusCode == HttpStatusCode.OK)
            {
                XDocument xdoc1 = XDocument.Parse(response1.Content.ReadAsStringAsync().Result);
                XmlDocument newXmlDocument1 = ConvertToXmlDocument(xdoc1);

                XmlNodeList ArticleList = (newXmlDocument1.SelectNodes("PubmedArticleSet/PubmedArticle"));
                dt = ProcessDetailNodes(ArticleList);
            }

            Context.Response.Write(JsonConvert.SerializeObject(dt));
        }

        public static DataTable ProcessDetailNodes(XmlNodeList nodes)
        {
            DataTable dtval = new DataTable();
            string keyword = "";
            string Author = "";
            dtval.Columns.Add(new DataColumn("Title", typeof(String)));
            dtval.Columns.Add(new DataColumn("ArticleTitle", typeof(String)));
            dtval.Columns.Add(new DataColumn("Name", typeof(String)));
            dtval.Columns.Add(new DataColumn("ISOAbbreviation", typeof(String)));
            dtval.Columns.Add(new DataColumn("PublicationDate", typeof(String)));
            dtval.Columns.Add(new DataColumn("Author", typeof(DataTable)));
            dtval.Columns.Add(new DataColumn("Keyword", typeof(DataTable)));
            dtval.Columns.Add(new DataColumn("link", typeof(DataTable)));

            dtval.Columns["Title"].DefaultValue = "";
            dtval.Columns["ArticleTitle"].DefaultValue = "";
            dtval.Columns["Name"].DefaultValue = "";
            dtval.Columns["ISOAbbreviation"].DefaultValue = "";
            dtval.Columns["PublicationDate"].DefaultValue = "";
            //dtval.Columns["link"].DefaultValue = "";
            //dtval.Columns["pdf"].DefaultValue = "";

            DataTable linkdt = new DataTable();
            linkdt.Columns.Add(new DataColumn("link", typeof(String)));
            linkdt.Columns.Add(new DataColumn("pdf", typeof(String)));

            DataTable keyworddt = new DataTable();
            keyworddt.Columns.Add(new DataColumn("keyword", typeof(String)));

            DataTable authordt = new DataTable();
            authordt.Columns.Add(new DataColumn("author", typeof(String)));


            for (int i = 0; i < nodes.Count; i++)
            {
                keyword = "";
                DataRow dr = dtval.NewRow();
                dtval.Rows.Add(dr);

                for (int k = 0; k < nodes[i].ChildNodes.Count; k++)
                {
                    if (nodes[i].ChildNodes[k].Name == "MedlineCitation")
                    {
                        for (int j = 0; j < nodes[i].ChildNodes[k].ChildNodes.Count; j++)
                        {
                            if (nodes[i].ChildNodes[k].ChildNodes[j].Name == "Article")
                            {
                                for (int m = 0; m < nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes.Count; m++)
                                {
                                    if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].Name == "Journal")
                                    {
                                        for (int n = 0; n < nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes.Count; n++)
                                        {
                                            if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].Name == "Title")
                                            {
                                                dtval.Rows[i]["Title"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].InnerText;
                                            }


                                            if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].Name == "ISOAbbreviation")
                                            {
                                                dtval.Rows[i]["ISOAbbreviation"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].InnerText;
                                            }
                                        }
                                    }

                                    if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].Name == "ArticleTitle")
                                    {
                                        dtval.Rows[i]["ArticleTitle"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].InnerText.ToString();
                                    }

                                    if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].Name == "Abstract")
                                    {
                                        dtval.Rows[i]["Name"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].InnerText.ToString();
                                    }
                                }
                            }

                            if (nodes[i].ChildNodes[k].ChildNodes[j].Name == "KeywordList")
                            {

                                for (int m = 0; m < nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes.Count; m++)
                                {
                                    DataRow dr1 = keyworddt.NewRow();
                                    keyworddt.Rows.Add(dr1);

                                    keyworddt.Rows[m]["keyword"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].InnerText.ToString();

                                }
                            }

                            dtval.Rows[i]["Keyword"] = keyworddt;

                            if (nodes[i].ChildNodes[k].ChildNodes[j].Name == "AuthorList")
                            {
                                for (int m = 0; m < nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes.Count; m++)
                                {
                                    DataRow dr1 = authordt.NewRow();
                                    authordt.Rows.Add(dr1);

                                    authordt.Rows[m]["keyword"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].InnerText.ToString();
                                }
                            }

                            dtval.Rows[i]["Author"] = authordt;
                        }
                    }
                    else if (nodes[i].ChildNodes[k].Name == "PubmedData")
                    {
                        for (int j = 0; j < nodes[i].ChildNodes[k].ChildNodes.Count; j++)
                        {
                            if (nodes[i].ChildNodes[k].ChildNodes[j].Name == "History")
                            {
                                int Year = Convert.ToInt32(nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[0].ChildNodes[0].InnerText);
                                int Month = Convert.ToInt32(nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[0].ChildNodes[1].InnerText);
                                int Day = Convert.ToInt32(nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[0].ChildNodes[2].InnerText);


                                dtval.Rows[i]["PublicationDate"] = new DateTime(Year, Month, Day);
                            }

                            if (nodes[i].ChildNodes[k].ChildNodes[j].Name == "ReferenceList")
                            {
                                string link = "";
                                string pdf = "";
                                for (int l = 0; l < nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes.Count; l++)
                                {
                                    if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[l].InnerText.Contains("http") || nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[l].InnerText.Contains("pdf"))
                                    {
                                        DataRow dr1 = linkdt.NewRow();
                                        linkdt.Rows.Add(dr1);
                                        if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[l].InnerText.Contains("http") && !nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[l].InnerText.Contains(".pdf"))
                                        {
                                            link = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[l].InnerText;
                                            link= link.Substring(link.LastIndexOf("http")).TrimEnd('.').TrimEnd(' ');
                                            if (link.Contains("["))
                                            {
                                                link = link.Substring(0,Convert.ToInt32(link.LastIndexOf("["))).TrimEnd('.').TrimEnd(' ');
                                            }
                                            linkdt.Rows[linkdt.Rows.Count-1]["link"] = link.TrimEnd('.').TrimEnd(' ');
                                        }

                                        if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[l].InnerText.Contains(".pdf"))
                                        {
                                            pdf = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[l].InnerText;
                                            linkdt.Rows[linkdt.Rows.Count - 1]["pdf"] = pdf.Substring(pdf.LastIndexOf("http"), Convert.ToInt32(pdf.LastIndexOf(".pdf")) - Convert.ToInt32(pdf.LastIndexOf("http"))) + ".pdf";
                                        }
                                    }
                                }

                                dtval.Rows[i]["link"] = linkdt;
                            }
                        }
                    }
                }
            }

            return dtval;
        }
    }
}

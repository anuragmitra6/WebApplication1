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
using System.CodeDom.Compiler;
using System.Diagnostics;

namespace WebApplication1
{
    public partial class SearchList : System.Web.UI.Page
    {
        public static DataTable newdt = new DataTable();
        public static string webtoken = "";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //Main Search
        [WebMethod(EnableSession = true)]
        public static string GetSearchList(string Test, string start, string sort, string max)
        {
            
            var xmlDocument = new XmlDocument();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            XmlDocument doc1 = new XmlDocument();

            string url = "https://eutils.ncbi.nlm.nih.gov/entrez/eutils/esearch.fcgi?db=pubmed&term=" + Test + "&usehistory=y&retstart=" + start + "&retmax=" + max + "&sort=" + sort + "";

            DataTable dt = new DataTable();
            DataTable dtnode = new DataTable();
            DataTable maindt = new DataTable();
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
                webtoken = Test;
                string sorting = sort.ToString();
                string qrylist = qrylistList[0].InnerText.ToString();
                string retmax = retmaxList[0].InnerText.ToString();
                string retstart = retstartList[0].InnerText.ToString();
                int Last = 9999 - Convert.ToInt32(retmax);
                int totalpage = 10000 / Convert.ToInt32(retmax);
                string rstart = "0";
                int next = Convert.ToInt32(retstart) + Convert.ToInt32(retmax);
                int page = next / Convert.ToInt32(retmax);
                int prev = 0;
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
                    dt = ProcessNodes(ArticleList, webenv, qrylist, retmax, next, prev, Test, rstart, Last, page, sorting, totalpage);
                }
            }

            return JsonConvert.SerializeObject(dt);
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

        public static DataTable ProcessNodes(XmlNodeList nodes, string webenv, string qrylist, string max, int total, int prev, string Term, string start, int Last, int page, string sorting, int totalpage)
        {
            DataTable dtval = new DataTable();
            string keyword = "";
            string Author = "";
            dtval.Columns.Add(new DataColumn("PMID", typeof(String)));
            dtval.Columns.Add(new DataColumn("Title", typeof(String)));
            dtval.Columns.Add(new DataColumn("ArticleTitle", typeof(String)));
            dtval.Columns.Add(new DataColumn("Abstract", typeof(String)));
            dtval.Columns.Add(new DataColumn("Author", typeof(String)));
            dtval.Columns.Add(new DataColumn("Volume", typeof(String)));
            dtval.Columns.Add(new DataColumn("Issue", typeof(String)));
            dtval.Columns.Add(new DataColumn("PubDate", typeof(String)));
            dtval.Columns.Add(new DataColumn("EPub", typeof(String)));
            dtval.Columns.Add(new DataColumn("PubType", typeof(String)));
            dtval.Columns.Add(new DataColumn("Pagination", typeof(String)));
            dtval.Columns.Add(new DataColumn("DOI", typeof(String)));
            dtval.Columns.Add(new DataColumn("Keyword", typeof(String)));
            dtval.Columns.Add(new DataColumn("ISOAbbreviation", typeof(String)));
            dtval.Columns.Add(new DataColumn("WebEnv", typeof(String)));
            dtval.Columns.Add(new DataColumn("QryList", typeof(String)));
            dtval.Columns.Add(new DataColumn("RetPrev", typeof(String)));
            dtval.Columns.Add(new DataColumn("RetNext", typeof(String)));
            dtval.Columns.Add(new DataColumn("Term", typeof(String)));
            dtval.Columns.Add(new DataColumn("Start", typeof(String)));
            dtval.Columns.Add(new DataColumn("Last", typeof(String)));
            dtval.Columns.Add(new DataColumn("Page", typeof(String)));
            dtval.Columns.Add(new DataColumn("sorting", typeof(String)));
            dtval.Columns.Add(new DataColumn("max", typeof(String)));
            dtval.Columns.Add(new DataColumn("totalpage", typeof(String)));

            for (int i = 0; i < nodes.Count; i++)
            {
                keyword = "";
                Author = "";
                DataRow dr = dtval.NewRow();
                dtval.Rows.Add(dr);

                dtval.Rows[i]["WebEnv"] = webenv;
                dtval.Rows[i]["QryList"] = qrylist;
                dtval.Rows[i]["RetNext"] = total;
                dtval.Rows[i]["RetPrev"] = prev;
                dtval.Rows[i]["Term"] = Term;
                dtval.Rows[i]["Start"] = start;
                dtval.Rows[i]["Last"] = Last;
                dtval.Rows[i]["Page"] = page.ToString();
                dtval.Rows[i]["sorting"] = sorting.ToString();
                dtval.Rows[i]["max"] = max.ToString();
                dtval.Rows[i]["totalpage"] = totalpage.ToString();

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

                                    if (nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].Name == "JournalIssue")
                                    {
                                        for (int j = 0; j < nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes.Count; j++)
                                        {
                                            if (nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes[j].Name == "Volume")
                                            {
                                                dtval.Rows[i]["Volume"] = nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes[j].InnerText;
                                            }

                                            if (nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes[j].Name == "Issue")
                                            {
                                                dtval.Rows[i]["Issue"] = nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes[j].InnerText;
                                            }

                                            if (nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes[j].Name == "PubDate")
                                            {
                                                string pubdate = "";
                                                for (int l = 0; l < nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes[j].ChildNodes.Count; l++)
                                                {
                                                    if (pubdate == "")
                                                    {
                                                        pubdate = nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes[j].ChildNodes[l].InnerText;
                                                    }
                                                    else
                                                    {
                                                        pubdate = pubdate + " " + nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes[j].ChildNodes[l].InnerText;
                                                    }
                                                }

                                                dtval.Rows[i]["PubDate"] = pubdate;
                                            }
                                        }
                                    }
                                }
                            }

                            if (nodes[i].ChildNodes[k].ChildNodes[m].Name == "ArticleTitle")
                            {
                                dtval.Rows[i]["ArticleTitle"] = nodes[i].ChildNodes[k].ChildNodes[m].InnerText.ToString();
                            }
                            if (nodes[i].ChildNodes[k].ChildNodes[m].Name == "Pagination")
                            {
                                for (int j = 0; j < nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes.Count; j++)
                                {
                                    if (nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[j].Name == "MedlinePgn")
                                    {
                                        dtval.Rows[i]["Pagination"] = nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[j].InnerText.ToString();
                                        break;
                                    }
                                }
                            }

                            if (nodes[i].ChildNodes[k].ChildNodes[m].Name == "ELocationID")
                            {
                                for (int j = 0; j < nodes[i].ChildNodes[k].ChildNodes[m].Attributes.Count; j++)
                                {
                                    if (nodes[i].ChildNodes[k].ChildNodes[m].Attributes[j].Value == "doi")
                                    {
                                        dtval.Rows[i]["DOI"] = nodes[i].ChildNodes[k].ChildNodes[m].InnerText.ToString();
                                    }
                                }
                            }

                            if (nodes[i].ChildNodes[k].ChildNodes[m].Name == "Abstract")
                            {
                                dtval.Rows[i]["Abstract"] = nodes[i].ChildNodes[k].ChildNodes[m].InnerText.ToString();
                            }

                            if (nodes[i].ChildNodes[k].ChildNodes[m].Name == "AuthorList")
                            {
                                for (int n = 0; n < nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes.Count; n++)
                                {
                                    string LastName = "";
                                    string Initials = "";
                                    if (nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].Name == "Author")
                                    {
                                        for (int j = 0; j < nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes.Count; j++)
                                        {
                                            if (nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes[j].Name == "LastName")
                                            {
                                                LastName = nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes[j].InnerText.ToString();
                                            }
                                            if (nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes[j].Name == "Initials")
                                            {
                                                Initials = nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes[j].InnerText.ToString();
                                            }
                                        }
                                    }
                                    if (Author == "")
                                    {
                                        Author = LastName + " " + Initials;
                                    }
                                    else
                                    {
                                        Author = Author + " , " + LastName + " " + Initials;
                                    }
                                }
                                dtval.Rows[i]["Author"] = Author;
                                //authordt.Rows[authordt.Rows.Count - 1]["Author"] 
                            }



                            if (nodes[i].ChildNodes[k].ChildNodes[m].Name == "PublicationTypeList")
                            {
                                string PubType = "";
                                for (int j = 0; j < nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes.Count; j++)
                                {
                                    PubType += " " + nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[j].InnerText;
                                }
                                dtval.Rows[i]["PubType"] = PubType;
                            }

                            if (nodes[i].ChildNodes[k].ChildNodes[m].Name == "ArticleDate")
                            {
                                string EPub = "";
                                for (int l = 0; l < nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes.Count; l++)
                                {
                                    if (EPub == "")
                                    {
                                        EPub = nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[l].InnerText;
                                    }
                                    else
                                    {
                                        EPub = EPub + " " + nodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[l].InnerText;
                                    }
                                }

                                dtval.Rows[i]["EPub"] = EPub;
                            }
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

        //Details from XML
        [WebMethod(EnableSession = true)]
        public static string GetDetails(string PMID, string WebEnv, string QryList)
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

            return JsonConvert.SerializeObject(dt);
        }

        public static DataTable ProcessDetailNodes(XmlNodeList nodes)
        {
            DataTable dtval = new DataTable();

            dtval.Columns.Add(new DataColumn("PMID", typeof(String)));
            dtval.Columns.Add(new DataColumn("Title", typeof(String)));
            dtval.Columns.Add(new DataColumn("ArticleTitle", typeof(String)));

            dtval.Columns.Add(new DataColumn("ISOAbbreviation", typeof(String)));
            dtval.Columns.Add(new DataColumn("PublicationDate", typeof(String)));
            dtval.Columns.Add(new DataColumn("Volume", typeof(String)));
            dtval.Columns.Add(new DataColumn("Issue", typeof(String)));
            dtval.Columns.Add(new DataColumn("EPub", typeof(String)));
            dtval.Columns.Add(new DataColumn("PubType", typeof(String)));
            dtval.Columns.Add(new DataColumn("Pagination", typeof(String)));
            dtval.Columns.Add(new DataColumn("DOI", typeof(String)));
            dtval.Columns.Add(new DataColumn("Citiation", typeof(String)));
            dtval.Columns.Add(new DataColumn("Author", typeof(String)));
            dtval.Columns.Add(new DataColumn("Name", typeof(DataTable)));
            dtval.Columns.Add(new DataColumn("Keyword", typeof(DataTable)));
            dtval.Columns.Add(new DataColumn("Affiliation", typeof(DataTable)));
            dtval.Columns.Add(new DataColumn("link", typeof(DataTable)));

            dtval.Columns["PMID"].DefaultValue = "";
            dtval.Columns["Title"].DefaultValue = "";
            dtval.Columns["ArticleTitle"].DefaultValue = "";
            dtval.Columns["ISOAbbreviation"].DefaultValue = "";
            dtval.Columns["PublicationDate"].DefaultValue = "";
            dtval.Columns["Citiation"].DefaultValue = "";
            dtval.Columns["Volume"].DefaultValue = "";
            dtval.Columns["Issue"].DefaultValue = "";
            dtval.Columns["EPub"].DefaultValue = "";
            dtval.Columns["PubType"].DefaultValue = "";
            dtval.Columns["Pagination"].DefaultValue = "";
            dtval.Columns["DOI"].DefaultValue = "";
            dtval.Columns["Author"].DefaultValue = "";

            DataTable linkdt = new DataTable();
            linkdt.Columns.Add(new DataColumn("linkdet", typeof(String)));
            linkdt.Columns.Add(new DataColumn("link", typeof(String)));
            linkdt.Columns.Add(new DataColumn("pdf", typeof(String)));

            linkdt.Columns["linkdet"].DefaultValue = "";
            linkdt.Columns["link"].DefaultValue = "";
            linkdt.Columns["pdf"].DefaultValue = "";

            DataTable keyworddt = new DataTable();
            keyworddt.Columns.Add(new DataColumn("keyword", typeof(String)));

            DataTable authordt = new DataTable();
            authordt.Columns.Add(new DataColumn("Affiliation", typeof(String)));

            DataTable abstdt = new DataTable();
            abstdt.Columns.Add(new DataColumn("INTRODUCTION", typeof(String)));
            abstdt.Columns.Add(new DataColumn("OBJECTIVE", typeof(String)));
            abstdt.Columns.Add(new DataColumn("STATEOFKNOWLEDGE", typeof(String)));
            abstdt.Columns.Add(new DataColumn("CONCLUSIONS", typeof(String)));

            abstdt.Columns["INTRODUCTION"].DefaultValue = "";
            abstdt.Columns["OBJECTIVE"].DefaultValue = "";
            abstdt.Columns["STATEOFKNOWLEDGE"].DefaultValue = "";
            abstdt.Columns["CONCLUSIONS"].DefaultValue = "";

            string Author = "";
            for (int i = 0; i < nodes.Count; i++)
            {
                DataRow dr = dtval.NewRow();
                dtval.Rows.Add(dr);

                for (int k = 0; k < nodes[i].ChildNodes.Count; k++)
                {
                    if (nodes[i].ChildNodes[k].Name == "MedlineCitation")
                    {
                        for (int j = 0; j < nodes[i].ChildNodes[k].ChildNodes.Count; j++)
                        {
                            if (nodes[i].ChildNodes[k].ChildNodes[j].Name == "PMID")
                            {
                                dtval.Rows[i]["PMID"] = nodes[i].ChildNodes[k].ChildNodes[j].InnerText;
                            }


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

                                            if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].Name == "JournalIssue")
                                            {
                                                for (int l = 0; l < nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].ChildNodes.Count; l++)
                                                {
                                                    if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].ChildNodes[l].Name == "Volume")
                                                    {
                                                        dtval.Rows[i]["Volume"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].ChildNodes[l].InnerText;
                                                    }

                                                    if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].ChildNodes[l].Name == "Issue")
                                                    {
                                                        dtval.Rows[i]["Issue"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].ChildNodes[l].InnerText;
                                                    }

                                                    if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].ChildNodes[l].Name == "PubDate")
                                                    {
                                                        string pubdate = "";
                                                        for (int a = 0; a < nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].ChildNodes[l].ChildNodes.Count; a++)
                                                        {
                                                            if (pubdate == "")
                                                            {
                                                                pubdate = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].ChildNodes[l].ChildNodes[a].InnerText;
                                                            }
                                                            else
                                                            {
                                                                pubdate = pubdate + " " + nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].ChildNodes[l].ChildNodes[a].InnerText;
                                                            }
                                                        }

                                                        dtval.Rows[i]["PublicationDate"] = pubdate;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].Name == "ArticleTitle")
                                    {
                                        dtval.Rows[i]["ArticleTitle"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].InnerText.ToString();
                                    }

                                    if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].Name == "Pagination")
                                    {
                                        for (int l = 0; l < nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes.Count; l++)
                                        {
                                            if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[l].Name == "MedlinePgn")
                                            {
                                                dtval.Rows[i]["Pagination"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[l].InnerText.ToString();
                                                break;
                                            }
                                        }
                                    }

                                    if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].Name == "ELocationID")
                                    {
                                        for (int l = 0; l < nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].Attributes.Count; l++)
                                        {
                                            if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].Attributes[l].Value == "doi")
                                            {
                                                dtval.Rows[i]["DOI"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].InnerText.ToString();
                                            }
                                        }
                                    }

                                    if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].Name == "Abstract")
                                    {
                                        string abstracts = "";
                                        if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes.Count > 1)
                                        {
                                            DataRow dr1 = abstdt.NewRow();
                                            abstdt.Rows.Add(dr1);

                                            for (int l = 0; l < nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes.Count; l++)
                                            {
                                                if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[l].Attributes.Count > 0)
                                                {
                                                    for (int a = 0; a < nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[l].Attributes.Count; a++)
                                                    {
                                                        if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[l].Attributes[a].Value == "INTRODUCTION")
                                                        {
                                                            abstdt.Rows[abstdt.Rows.Count - 1]["INTRODUCTION"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[l].InnerText.ToString();
                                                        }
                                                        else if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[l].Attributes[a].Value == "OBJECTIVE")
                                                        {
                                                            abstdt.Rows[abstdt.Rows.Count - 1]["OBJECTIVE"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[l].InnerText.ToString();
                                                        }

                                                        else if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[l].Attributes[a].Value == "STATE OF KNOWLEDGE")
                                                        {
                                                            abstdt.Rows[abstdt.Rows.Count - 1]["STATEOFKNOWLEDGE"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[l].InnerText.ToString();
                                                        }

                                                        else if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[l].Attributes[a].Value == "CONCLUSIONS")
                                                        {
                                                            abstdt.Rows[abstdt.Rows.Count - 1]["CONCLUSIONS"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[l].InnerText.ToString();
                                                        }
                                                        else
                                                        {
                                                            abstracts += nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[l].InnerText.ToString();
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    abstracts += nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[l].InnerText.ToString();
                                                }
                                            }
                                            if (abstracts != "")
                                            {
                                                abstdt.Rows[abstdt.Rows.Count - 1]["INTRODUCTION"] += abstracts;
                                            }
                                        }
                                        else
                                        {
                                            DataRow dr1 = abstdt.NewRow();
                                            abstdt.Rows.Add(dr1);

                                            abstdt.Rows[abstdt.Rows.Count - 1]["INTRODUCTION"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].InnerText.ToString();
                                        }

                                        dtval.Rows[i]["Name"] = abstdt;
                                    }

                                    if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].Name == "AuthorList")
                                    {
                                        Author = "";
                                        for (int n = 0; n < nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes.Count; n++)
                                        {
                                            string LastName = "";
                                            string Initials = "";
                                            if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].Name == "Author")
                                            {
                                                for (int l = 0; l < nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].ChildNodes.Count; l++)
                                                {
                                                    if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].ChildNodes[l].Name == "LastName")
                                                    {
                                                        LastName = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].ChildNodes[l].InnerText.ToString();
                                                    }
                                                    if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].ChildNodes[l].Name == "ForeName")
                                                    {
                                                        Initials = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].ChildNodes[l].InnerText.ToString();
                                                    }

                                                    if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].ChildNodes[l].Name == "AffiliationInfo")
                                                    {

                                                        DataRow dr1 = authordt.NewRow();
                                                        authordt.Rows.Add(dr1);

                                                        authordt.Rows[authordt.Rows.Count - 1]["Affiliation"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[n].ChildNodes[l].InnerText.ToString();
                                                    }
                                                }

                                                if (Author == "")
                                                {
                                                    Author = LastName + " " + Initials;
                                                }
                                                else
                                                {
                                                    Author = Author + " , " + LastName + " " + Initials;
                                                }
                                            }

                                        }
                                        dtval.Rows[i]["Author"] = Author;
                                        dtval.Rows[i]["Affiliation"] = authordt;
                                    }

                                    if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].Name == "PublicationTypeList")
                                    {
                                        string PubType = "";
                                        for (int l = 0; l < nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes.Count; l++)
                                        {
                                            PubType += " " + nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[l].InnerText;
                                        }
                                        dtval.Rows[i]["PubType"] = PubType;
                                    }

                                    if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].Name == "ArticleDate")
                                    {
                                        string EPub = "";
                                        for (int l = 0; l < nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes.Count; l++)
                                        {
                                            if (EPub == "")
                                            {
                                                EPub = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[l].InnerText;
                                            }
                                            else
                                            {
                                                EPub = EPub + " " + nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].ChildNodes[l].InnerText;
                                            }
                                        }

                                        dtval.Rows[i]["EPub"] = EPub;
                                    }
                                }
                            }

                            if (nodes[i].ChildNodes[k].ChildNodes[j].Name == "KeywordList")
                            {

                                for (int m = 0; m < nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes.Count; m++)
                                {
                                    DataRow dr1 = keyworddt.NewRow();
                                    keyworddt.Rows.Add(dr1);

                                    keyworddt.Rows[keyworddt.Rows.Count - 1]["keyword"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[m].InnerText.ToString();

                                }
                            }

                            dtval.Rows[i]["Keyword"] = keyworddt;





                        }
                    }
                    else if (nodes[i].ChildNodes[k].Name == "PubmedData")
                    {
                        for (int j = 0; j < nodes[i].ChildNodes[k].ChildNodes.Count; j++)
                        {
                            if (nodes[i].ChildNodes[k].ChildNodes[j].Name == "ReferenceList")
                            {
                                string link = "";
                                string pdf = "";

                                dtval.Rows[i]["Citiation"] = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes.Count;

                                for (int l = 0; l < nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes.Count; l++)
                                {
                                    if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[l].InnerText.Contains("http") || nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[l].InnerText.Contains("pdf"))
                                    {
                                        DataRow dr1 = linkdt.NewRow();
                                        linkdt.Rows.Add(dr1);
                                        if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[l].InnerText.Contains("http") && !nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[l].InnerText.Contains(".pdf"))
                                        {
                                            link = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[l].InnerText;
                                            linkdt.Rows[linkdt.Rows.Count - 1]["linkdet"] = link.Substring(0, link.LastIndexOf("http")).TrimEnd('.').TrimEnd(' ');
                                            link = link.Substring(link.LastIndexOf("http")).TrimEnd('.').TrimEnd(' ');

                                            if (link.Contains("["))
                                            {
                                                link = link.Substring(0, Convert.ToInt32(link.LastIndexOf("["))).TrimEnd('.').TrimEnd(' ');
                                            }
                                            linkdt.Rows[linkdt.Rows.Count - 1]["link"] = link.TrimEnd('.').TrimEnd(' ');
                                        }

                                        if (nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[l].InnerText.Contains(".pdf"))
                                        {
                                            try
                                            {
                                                pdf = nodes[i].ChildNodes[k].ChildNodes[j].ChildNodes[l].InnerText;
                                                linkdt.Rows[linkdt.Rows.Count - 1]["pdf"] = pdf.Substring(pdf.LastIndexOf("http"), Convert.ToInt32(pdf.LastIndexOf(".pdf")) - Convert.ToInt32(pdf.LastIndexOf("http"))) + ".pdf";

                                            }
                                            catch (Exception ex) { }
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

        //-----------------------------------------punita-------------------------------------

            //Pdf SEarch
        [WebMethod(EnableSession = true)]
        public static string GetSearchListpdf(string Test)
        {
            //if (newdt.Rows.Count == 0)
            //{
            string url = "https://eutils.ncbi.nlm.nih.gov/entrez/eutils/esearch.fcgi?db=pubmed&term=" + webtoken + "&usehistory=y&retstart=1&retmax=200&sort=relevance";

            DataTable dt = new DataTable();
            DataTable dtnode = new DataTable();
            DataTable maindt = new DataTable();
            HttpClient client1 = new HttpClient();
            client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            HttpResponseMessage response = client1.GetAsync(url).Result;

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
                string url1 = "https://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db=pubmed&query_key=1&WebEnv=" + webenv + "&retstart=1&retmax=200&retmode=xml&rettype=abstract";

                HttpResponseMessage response1 = client1.GetAsync(url1).Result;
                if (response1.StatusCode == HttpStatusCode.OK)
                {
                    XDocument xdoc11 = XDocument.Parse(response1.Content.ReadAsStringAsync().Result);
                    XmlDocument newXmlDocument11 = ConvertToXmlDocument(xdoc11);

                    XmlNodeList ArticleList1 = (newXmlDocument11.SelectNodes("PubmedArticleSet/PubmedArticle/PubmedData/ReferenceList"));
                    newdt = ProcessDownloadpdf(ArticleList1);
                }
            }
            //}

            DataTable dt2 = new DataTable();
            dt2.Merge(newdt);

            DataTable dtcheck = new DataTable();
            dtcheck.Columns.Add(new DataColumn("foundpdf", typeof(String)));
            dtcheck.Columns.Add(new DataColumn("notpdf", typeof(String)));

            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                string path = dt2.Rows[i]["pdf"].ToString();
                if (File.Exists(path))
                {
                    int result = SearchPdfFile(path, Test);
                    //DataRow 
                    if (result == 1)
                    {
                        dtcheck.Rows.Add(dt2.Rows[i]["linkdet"].ToString(), "");
                    }
                    else if (result == 2)
                    {
                        dtcheck.Rows.Add("", dt2.Rows[i]["linkdet"].ToString());
                    }

                }
            }

            return JsonConvert.SerializeObject(dtcheck);
        }

        public static DataTable ProcessDownloadpdf(XmlNodeList nodes)
        {
            DataTable linkdt = new DataTable();
            linkdt.Columns.Add(new DataColumn("linkdet", typeof(String)));
            linkdt.Columns.Add(new DataColumn("link", typeof(String)));
            linkdt.Columns.Add(new DataColumn("pdf", typeof(String)));
            linkdt.Columns["linkdet"].DefaultValue = "";
            linkdt.Columns["link"].DefaultValue = "";
            linkdt.Columns["pdf"].DefaultValue = "";
            DataTable dtval1 = new DataTable();

            //ankita next task for 07-03-2023
            var root = HttpContext.Current.Server.MapPath("PDFDirectory");
            bool exists = System.IO.Directory.Exists(root);

            if (!exists)
                System.IO.Directory.CreateDirectory(root);

            System.IO.DirectoryInfo di = new DirectoryInfo(root);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            for (int i = 0; i < nodes.Count; i++)
            {


                for (int l = 0; l < nodes[i].ChildNodes.Count; l++)
                {
                    string pdf = "";
                    //if (nodes[i].ChildNodes[l].InnerText.Contains("http") || nodes[i].ChildNodes[l].InnerText.Contains("pdf"))
                    //{


                    if (nodes[i].ChildNodes[l].InnerText.Contains(".pdf"))
                    {

                        pdf = nodes[i].ChildNodes[l].InnerText;
                        try
                        {
                            string pdglink = pdf.Substring(pdf.LastIndexOf("http"), Convert.ToInt32(pdf.LastIndexOf(".pdf")) - Convert.ToInt32(pdf.LastIndexOf("http"))) + ".pdf";
                            string[] s = pdglink.Split('/');
                            string filename = s[s.Length - 1];


                            

                            string path = root + "\\" + filename;
                            //Create file and write to it

                            //using (var fs = new FileStream(path,FileAccess.Read))
                            //{
                            //    using (var str = new StreamWriter(fs))
                            //    {
                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFile(pdglink, root + "\\" + filename);
                            }
                            linkdt.Rows.Add(pdglink, filename, root + "\\" + filename);
                            //        str.Flush();
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {

                        }
                        //}
                    }
                }//Child and adolescent mental health
            }
            return linkdt;
        }

        public static int SearchPdfFile(string fileName, String searchText)
        {
            int res = 0;
            try
            {

                if (!File.Exists(fileName))
                    throw new FileNotFoundException("File not found", fileName);

                iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(fileName);
                iTextSharp.text.pdf.parser.ITextExtractionStrategy strategy = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();

                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    var currentText = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, page, strategy);
                    if (currentText.Contains(searchText))
                    {
                        res = 1;
                        break;
                    }

                }

                if (res == 0)
                {
                    res = 2;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                res = 0;
            }
            return res;
        }
    }
}
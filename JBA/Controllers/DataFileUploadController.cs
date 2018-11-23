using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace JBA.Controllers
{
    public class DataFileUploadController : Controller
    {
        //
        // GET: /DataFileUpload/


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadedData(int? page)
        {

            if (page == null)
            {
                page = 1;
            }

            List<JBA.Models.SimpleData> dataResults = new List<Models.SimpleData>();
            using (var db = new JBAContext())
            {
                dataResults = db.SimpleDataCollection.ToList();
                
            }

            int pageSize = 200;
            int pageNumber = (page ?? 1);
            return View(dataResults.ToPagedList(pageNumber, pageSize));

        }

        [HttpPost]
        public ActionResult DataFileUpload(string txtRowNumbersDescription, string txtRowNumbersHeaders, string txtDataSeperatedBy, string txtDataStartsAtRow)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    ProcessData(file, txtRowNumbersDescription, txtRowNumbersHeaders, txtDataSeperatedBy, txtDataStartsAtRow);

                }
            }

            return RedirectToAction("UploadedData");
        }

        private void ProcessData(HttpPostedFileBase file, string txtRowNumbersDescription, string txtRowNumbersHeaders, string txtDataSeperatedBy, string txtDataStartsAtRow)
        {


            bool hasDescription = false;
            bool hasHeaders = false;

            // Read the file into a list of strings for easier manipulation
            List<string> fileRows = new List<string>();
            string currentLine = string.Empty;

            using (var srFileContents = new StreamReader(file.InputStream))
            {
                while ((currentLine = srFileContents.ReadLine()) != null)
                {
                    fileRows.Add(currentLine);
                }
            }


            // check that there are actual rows in the file
            if (fileRows.Count > 0)
            {
                System.Web.HttpContext.Current.Session["totalrows"] = fileRows.Count;

                // Initialise stringbuilders for the Description and the Headers
                StringBuilder sbDescription = new StringBuilder();
                List<int> descriptionRows = new List<int>();

                StringBuilder sbHeaders = new StringBuilder();
                List<int> headerRows = new List<int>();

                // Get the list of row numbers specified for the description
                if (!String.IsNullOrEmpty(txtRowNumbersDescription))
                {
                    try
                    {
                        descriptionRows = txtRowNumbersDescription.Split(',').Select(x => int.Parse(x)).ToList();
                    }
                    catch (Exception ex)
                    {
                        // description row values could not be parsed
                    }

                    foreach (int i in descriptionRows)
                    {

                        //if the row number specified in the description rows exists in the list of rows of the file
                        if (fileRows.ElementAtOrDefault((i - 1)) != null)
                        {
                            sbDescription.Append(fileRows[i - 1]);
                            if (!hasDescription)
                            {
                                hasDescription = true;
                            }
                        }
                        else
                        {
                            //this row number does not exist
                        }

                    }
                }

                // Get the list of row numbers specified for the headers
                if (!String.IsNullOrEmpty(txtRowNumbersHeaders))
                {
                    try
                    {
                        headerRows = txtRowNumbersHeaders.Split(',').Select(x => int.Parse(x)).ToList();
                    }
                    catch (Exception ex)
                    {
                        // header row values could not be parsed
                    }
                    //Get the headers
                    foreach (int i in headerRows)
                    {
                        //if the row number specified in the header rows exists in the list of rows of the file
                        if (fileRows.ElementAtOrDefault((i - 1)) != null)
                        {
                            sbHeaders.Append(fileRows[i - 1]);
                            if (!hasHeaders)
                            {
                                hasHeaders = true;
                            }
                        }
                        else
                        {
                            //this row number does not exist
                        }
                    }
                }



                bool canProceed = false;
                int startYear = 0;
                int endYear = 0;

                if (hasHeaders)
                {


                    

                    //Parse the headers for years data
                    var rgxHeaders = new Regex("\\[years=([12][\\d]{3})\\-([12][\\d]{3})\\]{1}", RegexOptions.IgnoreCase);
                    var yearsMatch = rgxHeaders.Matches(sbHeaders.ToString());

                    if (yearsMatch.Count == 1)
                    {
                        if (int.TryParse(yearsMatch[0].Groups[1].Value, out startYear) && int.TryParse(yearsMatch[0].Groups[2].Value, out endYear))
                        {
                            if (startYear < endYear)
                            {
                                canProceed = true;

                            }
                            else
                            {
                                // End year is before start year
                            }

                        }
                    }
                    else
                    {
                        // years incorrect, cannot proceed
                    }
                }
                else
                {
                    // There are no headers so there is no year information
                }


                // Got header information, then we can proceed
                if (canProceed)
                {
                    int dataStartsAtRow = -1;
                    int currentGridX = 0;
                    int currentGridY = 0;
                    int currentYear = DateTime.MinValue.Year;


                    if (int.TryParse(txtDataStartsAtRow, out dataStartsAtRow))
                    {
                        if (fileRows.ElementAtOrDefault((dataStartsAtRow - 1)) != null)
                        {
                            // Regex for seperator
                            var rgxGridRef = new Regex("grid-ref=\\s*(\\d+)\\s*,\\s*(\\d+)", RegexOptions.IgnoreCase);
                            // Regex for data
                            var rgxData = new Regex("^[\\t ]*(\\d+)[\\t ]+(\\d+)[\\t ]+(\\d+)[\\t ]+(\\d+)[\\t ]+(\\d+)[\\t ]+(\\d+)[\\t ]+(\\d+)[\\t ]+(\\d+)[\\t ]+(\\d+)[\\t ]+(\\d+)[\\t ]+(\\d+)[\\t ]+(\\d+)[\\t ]*$", RegexOptions.IgnoreCase);

                            foreach (var line in fileRows.Skip(dataStartsAtRow - 1))
                            {

                                System.Web.HttpContext.Current.Session["currentrow"] = fileRows.IndexOf(line) + 1;

                                var matchesGridRef = rgxGridRef.Matches(line);
                                var matchesData = rgxData.Matches(line);

                                // On a gridRef row
                                if (matchesGridRef.Count > 0)
                                {
                                    currentYear = startYear;

                                    if (!int.TryParse(matchesGridRef[0].Groups[1].Value, out currentGridX))
                                    {
                                        currentGridX = -99999;
                                    }
                                    if (!int.TryParse(matchesGridRef[0].Groups[2].Value, out currentGridY))
                                    {
                                        currentGridY = -99999;
                                    }
                                }

                                // On a data row
                                if (matchesData.Count > 0)
                                {
                                    Dictionary<int, int> monthValue = new Dictionary<int, int>();
                                    int jan = -99999;
                                    int feb = -99999;
                                    int mar = -99999;
                                    int apr = -99999;
                                    int may = -99999;
                                    int jun = -99999;
                                    int jul = -99999;
                                    int aug = -99999;
                                    int sep = -99999;
                                    int oct = -99999;
                                    int nov = -99999;
                                    int dec = -99999;


                                    // If there any parsing issues we will store the value as a large -99999 value to identify that there might be an issue
                                    //JAN DATA
                                    int.TryParse(matchesData[0].Groups[1].Value, out jan);
                                    monthValue.Add(1, jan);

                                    //FEB DATA
                                    int.TryParse(matchesData[0].Groups[2].Value, out feb);
                                    monthValue.Add(2, feb);

                                    //MAR DATA
                                    int.TryParse(matchesData[0].Groups[3].Value, out mar);
                                    monthValue.Add(3, mar);


                                    //APR DATA
                                    int.TryParse(matchesData[0].Groups[4].Value, out apr);
                                    monthValue.Add(4, jan);

                                    //MAY DATA
                                    int.TryParse(matchesData[0].Groups[5].Value, out may);
                                    monthValue.Add(5, feb);

                                    //JUN DATA
                                    int.TryParse(matchesData[0].Groups[6].Value, out jun);
                                    monthValue.Add(6, feb);


                                    //JUL DATA
                                    int.TryParse(matchesData[0].Groups[7].Value, out jul);
                                    monthValue.Add(7, jan);

                                    //AUG DATA
                                    int.TryParse(matchesData[0].Groups[8].Value, out aug);
                                    monthValue.Add(8, feb);

                                    //SEP DATA
                                    int.TryParse(matchesData[0].Groups[9].Value, out sep);
                                    monthValue.Add(9, feb);


                                    //OCT DATA
                                    int.TryParse(matchesData[0].Groups[10].Value, out oct);
                                    monthValue.Add(10, jan);

                                    //NOV DATA
                                    int.TryParse(matchesData[0].Groups[11].Value, out nov);
                                    monthValue.Add(11, feb);

                                    //DEC DATA
                                    int.TryParse(matchesData[0].Groups[12].Value, out dec);
                                    monthValue.Add(12, feb);

                                    List<JBA.Models.SimpleData> lstSimpleData = new List<Models.SimpleData>();

                                    foreach (KeyValuePair<int, int> entry in monthValue)
                                    {
                                        var simpleData = new JBA.Models.SimpleData
                                        {
                                            XRef = currentGridX,
                                            YRef = currentGridY,
                                            Date = new DateTime(currentYear, entry.Key, 1),
                                            Value = entry.Value
                                        };
                                        lstSimpleData.Add(simpleData);

                                    }

                                    if (lstSimpleData.Count > 0)
                                    {
                                        try
                                        {
                                            using (var db = new JBAContext())
                                            {
                                                db.SimpleDataCollection.AddRange(lstSimpleData);
                                                db.SaveChanges();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            // Couldn't connect to Database Context
                                        }
                                        finally
                                        {
                                            currentYear++;
                                        }
                                    }
                                    else
                                    {
                                        // No rows to add
                                    }
                                }
                                else
                                {
                                    // No matched DATA 
                                }
                            }
                        }
                        else
                        {
                            // Data starting row is not in the list of rows
                        }

                    }
                    else
                    {
                        //Invalid value for data starting row

                    }
                }
            }
            else
            {
                // There are no rows to process
            }
            
        }

    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOSBPM.Models;
using Newtonsoft.Json;

namespace DOSBPM.Controllers
{
    public class PropertyOwnerContactController : BaseController
    {
        // GET: PropertyOwnerContact

        DEV_CODES_APPDBEntities db = new DEV_CODES_APPDBEntities();
        
        public ActionResult Index()
        {

            BuildingApplication buildApp = new BuildingApplication();
            PropertyOwnerContact propertyOwnerInfo = new PropertyOwnerContact();

            if (Session["BuildingApplication"] != null)
            {
                buildApp = (BuildingApplication)Session["BuildingApplication"];
            }
            else
            {
                string jsonData = string.Empty;
                Temp_BPMData objtemp_BPMData = db.Temp_BPMData.FirstOrDefault(x => x.AppID == "1" && x.UserID == "1");
                if (objtemp_BPMData != null)
                {
                    jsonData = objtemp_BPMData.JsonData;
                }
                buildApp = JsonConvert.DeserializeObject<BuildingApplication>(jsonData);
            }
            if (buildApp == null)
            {
                buildApp = new BuildingApplication();
            }

            //ViewBag.info = new List<SelectListItem> {
            //                                            new SelectListItem {Value="Property Owner Organization", Text="Property Owner Organization", Selected=(buildApp.PropertyOwnerInfoData?.PropertyOwner=="Property Owner Organization")},
            //                                            new SelectListItem {Value="Property Owner Individual", Text="Property Owner Individual", Selected=(buildApp.PropertyOwnerInfoData?.PropertyOwner=="Property Owner Individual")}

            //                                        };
            buildApp.PropertyOwnerContactData = (buildApp.PropertyOwnerContactData == null) ? new PropertyOwnerContact() : buildApp.PropertyOwnerContactData;
            buildApp.PropertyOwnerContactData.AddressInfo = (buildApp.PropertyOwnerContactData.AddressInfo == null) ? new AddressInfo() : buildApp.PropertyOwnerContactData.AddressInfo;

            buildApp.PropertyOwnerContactData.StakeholderTypeList = GetStakeholderTypes();
            
            AddressInfo objAddressInfo = new AddressInfo();
            buildApp.PropertyOwnerContactData.AddressInfo.CountryList = objAddressInfo.CountryList;
            buildApp.PropertyOwnerContactData.AddressInfo.StatesList = objAddressInfo.StatesList;
            buildApp.PropertyOwnerContactData.AddressInfo.CountiesList = objAddressInfo.CountiesList;
            buildApp.PropertyOwnerContactData.PropertyOwner = buildApp.PropertyOwnerInfoData.OrganizationName;

            return View(buildApp.PropertyOwnerContactData);


        }
        [HttpPost]
        public ActionResult Index(PropertyOwnerContact propertyOwnerContactInfo)
        {
            BuildingApplication buildApp = null;
            if (Session["BuildingApplication"] != null)
            {
                buildApp = (BuildingApplication)Session["BuildingApplication"];
            }
            else
            {
                buildApp = new BuildingApplication();
            }

            propertyOwnerContactInfo.AddressInfo.CountiesList = null;
            propertyOwnerContactInfo.AddressInfo.StatesList = null;
            propertyOwnerContactInfo.AddressInfo.CountryList = null;
            propertyOwnerContactInfo.StakeholderTypeList = null;

            if (buildApp.QualifyingInfoData != null)
            {
                buildApp.QualifyingInfoData.TransactionTypeList = null;
            }
            if(buildApp.PropertyOwnerInfoData != null)
            {
                
                    buildApp.PropertyOwnerInfoData.AddressInfo.CountiesList = null;
                    buildApp.PropertyOwnerInfoData.AddressInfo.StatesList = null;
                    buildApp.PropertyOwnerInfoData.AddressInfo.CountryList = null;
                    buildApp.PropertyOwnerInfoData.StakeholderTypeList = null;

                
            }

            buildApp.PropertyOwnerContactData = propertyOwnerContactInfo;
            Session["BuildingApplication"] = buildApp;

            string buildAppString = JsonConvert.SerializeObject(buildApp);

            Temp_BPMData objtemp_BPMData = db.Temp_BPMData.FirstOrDefault(x => x.AppID == "1" && x.UserID == "1");
            if (objtemp_BPMData != null)
            {
                objtemp_BPMData.AppID = "1";
                objtemp_BPMData.UserID = "1";
                objtemp_BPMData.JsonData = buildAppString;
                db.SaveChanges();
            }
            else
            {
                Temp_BPMData objtempBPM = new Temp_BPMData();
                objtempBPM.AppID = "1";
                objtempBPM.UserID = "1";

                objtempBPM.JsonData = buildAppString;
                db.Temp_BPMData.Add(objtempBPM);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "AdditionalStakeholderInfo");

            //if (propertyOwnerInfo.StakeholderType == "SH_1")
            //{
            //    return RedirectToAction("Index", "AdditionalStakeholderInfo");
            //}
            //else
            //{
            //    return RedirectToAction("Index", "AdditionalStakeholderInfo");
        }

    }
        //[HttpPost]
        //public JsonResult GetStackHolder(string stackHolder, string type)
        //{
        //   List<StackInfo> stackHolders = new List<StackInfo>();
        //    var resp = new PropertyInfoData();
        //    stackHolders = resp.GetStackInfoData(stackHolder, type);
        //    return Json(stackHolders, JsonRequestBehavior.AllowGet);
        //}

    
}
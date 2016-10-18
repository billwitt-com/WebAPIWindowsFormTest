using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRiverWatchApi.Helpers
{
    public class MakeICPInbound
    {
        public InboundICPFinal makeICP(string barcode, string type, int tableSampleID)
        {
            DateTime? newDate;
            Random RAND = new Random();
            InboundICPFinal INB = new InboundICPFinal(); // create new data entity
            RiverWatchEntities entities = new RiverWatchEntities();
            // get user input

            //     string numberSample = txtNumSmp.Text.Trim(); // really bad name, perhaps we will have time to correct           

            decimal mult = 0.0m;
            decimal V = 0;

            if (type.Substring(0, 1) == "0")  // normal sample
            {
                mult = 100;
            }
            if (type.Substring(0, 1) == "1")  // blank
            {
                mult = .1m;
            }

            if (type.Substring(0, 1) == "2")  // duplicate 
            {
                mult = 125;
            }

            try
            {
                INB.CODE = barcode;  // make new barcode
                INB.DUPLICATE = type; // this is poorly named
                INB.tblSampleID = tableSampleID;

                V = (decimal)(from z in entities.tlkLimits
                              where z.Element.ToUpper() == "AL"
                              select z.Reporting.Value).FirstOrDefault();

                INB.AL_D = (decimal)RAND.NextDouble() * V * mult;
                INB.AL_T = (decimal)INB.AL_D + (decimal)RAND.NextDouble() + .5m;

                V = (decimal)(from z in entities.tlkLimits
                              where z.Element.ToUpper() == "AS"
                              select z.Reporting.Value).FirstOrDefault();

                INB.AS_D = (decimal)RAND.NextDouble() * V * mult;
                INB.AS_T = (decimal)INB.AS_D + (decimal)RAND.NextDouble() + .5m;

                V = (decimal)(from z in entities.tlkLimits
                              where z.Element.ToUpper() == "CA"
                              select z.Reporting.Value).FirstOrDefault();

                INB.CA_D = (decimal)RAND.NextDouble() * V * mult;
                INB.CA_T = (decimal)INB.CA_D + (decimal)RAND.NextDouble() + .5m;

                V = (decimal)(from z in entities.tlkLimits
                              where z.Element.ToUpper() == "CD"
                              select z.Reporting.Value).FirstOrDefault();

                INB.CD_D = (decimal)RAND.NextDouble() * V * mult;
                INB.CD_T = (decimal)INB.CD_D + (decimal)RAND.NextDouble() + .5m;

                V = (decimal)(from z in entities.tlkLimits
                              where z.Element.ToUpper() == "CU"
                              select z.Reporting.Value).FirstOrDefault();
                INB.CU_D = (decimal)RAND.NextDouble() * V * mult;
                INB.CU_T = (decimal)INB.CU_D + (decimal)RAND.NextDouble() + .5m;

                V = (decimal)(from z in entities.tlkLimits
                              where z.Element.ToUpper() == "FE"
                              select z.Reporting.Value).FirstOrDefault();
                INB.FE_D = (decimal)RAND.NextDouble() * V * mult;
                INB.FE_T = (decimal)INB.FE_D + (decimal)RAND.NextDouble() + .5m;

                V = (decimal)(from z in entities.tlkLimits
                              where z.Element.ToUpper() == "K"
                              select z.Reporting.Value).FirstOrDefault();
                INB.K_D = (decimal)RAND.NextDouble() * V * mult;
                INB.K_T = (decimal)INB.K_D + (decimal)RAND.NextDouble() + .5m;

                V = (decimal)(from z in entities.tlkLimits
                              where z.Element.ToUpper() == "MG"
                              select z.Reporting.Value).FirstOrDefault();
                INB.MG_D = (decimal)RAND.NextDouble() * V * mult; ; // make Total_Dups smaller than Disolved_Dups 
                INB.MG_T = (decimal)INB.MG_D - (decimal)RAND.NextDouble() + .5m;

                V = (decimal)(from z in entities.tlkLimits
                              where z.Element.ToUpper() == "MN"
                              select z.Reporting.Value).FirstOrDefault();
                INB.MN_D = (decimal)RAND.NextDouble() * V * mult;
                INB.MN_T = (decimal)INB.MN_D + (decimal)RAND.NextDouble() - .5m; ;

                V = (decimal)(from z in entities.tlkLimits
                              where z.Element.ToUpper() == "NA"
                              select z.Reporting.Value).FirstOrDefault();
                INB.NA_D = (decimal)RAND.NextDouble() * V * mult;
                INB.NA_T = (decimal)INB.NA_D + (decimal)RAND.NextDouble() + .5m;

                V = (decimal)(from z in entities.tlkLimits
                              where z.Element.ToUpper() == "PB"
                              select z.Reporting.Value).FirstOrDefault();
                INB.PB_D = (decimal)RAND.NextDouble() * V * mult;
                INB.PB_T = (decimal)INB.PB_D + (decimal)RAND.NextDouble() + .5m;

                V = (decimal)(from z in entities.tlkLimits
                              where z.Element.ToUpper() == "SE"
                              select z.Reporting.Value).FirstOrDefault();
                INB.SE_D = (decimal)RAND.NextDouble() * V * mult;
                INB.SE_T = (decimal)INB.SE_D - (decimal)RAND.NextDouble() + .5m;

                V = (decimal)(from z in entities.tlkLimits
                              where z.Element.ToUpper() == "ZN"
                              select z.Reporting.Value).FirstOrDefault();

                INB.ZN_D = (decimal)RAND.NextDouble() * V * mult; ; // make Total_Dups smaller than Disolved_Dups 
                INB.ZN_T = (decimal)INB.ZN_D - .5m;

                INB.Comments = "Created by hand for testing";

                INB.ANADATE = DateTime.Now;

                newDate = DateTime.Now.AddDays(-2);

                INB.DATE_SENT = newDate;

                INB.CreatedBy = "Windows App Test System";

                newDate = DateTime.Now.AddDays(-6);
                INB.CreatedDate = newDate.Value;
                INB.COMPLETE = true;
                INB.Saved = false;
                INB.Edited = false;
                INB.Valid = true;
                return INB;
            }

            catch (Exception ex)
            {
                string nam = "ICP BC Generator";
                string msg = ex.Message;
                LogErrror LE = new LogErrror();
                LE.LogError(msg, "MakeIPCInbound", ex.StackTrace.ToString(), nam, "From Windows Form makeICPIbound Data generator");
                return INB; // for the compiler.. 
            }
        }
    }
}

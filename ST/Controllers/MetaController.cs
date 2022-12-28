using System;
using System.IO;
using System.Web;
using System.Net;
using System.Linq;
using System.Data;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using ExcelDataReader;
using System.Collections.Generic;
using System.Globalization;

namespace ST.Controllers
{
    //[Authorize]
    [EnableCors(origins: "http://localhost:4200 , http://ctec.support-royalticgroup.com", headers: "*", methods: "*")]
    [RoutePrefix("api/Meta")]
    public class MetaController : ApiController
    {
        // GET api/<controller>
        public Models.Meta objReferenceMethods = null;
        // public Models.ModelHealthAdvisor dbContext = new Models.ModelHealthAdvisor();

        /// <summary>
        /// List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        public Models.Results GetAll()
        {
            Models.Results resultado = new Models.Results();
            //if (TokenGenerator.HasPermissions(Request, "HA_SITE_GROUP", "Read"))
            //{
            try
            {
                using (var dbContextReferenceMethods = new Models.ModelHealthAdvisor())
                {
                    var months = Enumerable.Range(1, 12).Select(i => new { I = i, M = DateTimeFormatInfo.CurrentInfo.GetMonthName(i) }).ToList();

                    var data = dbContextReferenceMethods.MetaDetalle
                    .Include("Meta")
                    //.AsEnumerable()
                    //.Where(t => t.refDetIsDeleted != true)
                    .Select(x => new
                    {
                        x.MetaDetalleId,
                        x.Meta.LocalId,
                        LocalNombre = dbContextReferenceMethods.LocalesNacionales.FirstOrDefault(y => y.CodigoLocal == x.Meta.LocalId).NombreLocal,
                        //Mes = months.FirstOrDefault(a => a.I == x.Mes).M,// string.Format("{0:MMMM}", x.Mes),
                        Mes = x.Mes,
                        x.PorcentajeCe,
                        x.PorcentajeCh,
                        x.PorcentajeSe
                    })
                    .Where(z=>z.PorcentajeCe>0)
                    .ToList();

                    resultado.OBJETO = data.Select(x => new
                    {
                        x.MetaDetalleId,
                        x.LocalId,
                        x.LocalNombre,
                        //Mes = months.FirstOrDefault(a => a.I == x.Mes).M,// string.Format("{0:MMMM}", x.Mes),
                        Mes = months.FirstOrDefault(a => a.I == x.Mes).M,
                        x.PorcentajeCe,
                        x.PorcentajeCh,
                        x.PorcentajeSe
                    })
                    .Where(z => z.PorcentajeCe > 0)
                    .ToList();

                    resultado.MENSAJE = "Listado de reference methods consultadas exitosamente";
                    resultado.STATUS = "success";
                    return resultado;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
            //}
            //else
            //{
            //    resultado.OBJETO = null;
            //    resultado.MENSAJE = "No authorized";
            //    resultado.STATUS = "unsuccess";
            //    return resultado;
            //}





        }

        [Route("ReadFile")]
        [HttpPost]
        public string ReadFile()
        {
            try
            {
                #region Variable Declaration
                string message = "";
                HttpResponseMessage ResponseMessage = null;
                var httpRequest = HttpContext.Current.Request;
                DataSet dsexcelRecords = new DataSet();
                IExcelDataReader reader = null;
                HttpPostedFile Inputfile = null;
                Stream FileStream = null;
                #endregion

                #region Save Data Detail From Excel
                using (var objEntity = new Models.ModelHealthAdvisor())
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        Inputfile = httpRequest.Files[0];
                        FileStream = Inputfile.InputStream;

                        if (Inputfile != null && FileStream != null)
                        {
                            if (Inputfile.FileName.EndsWith(".xls"))
                                reader = ExcelReaderFactory.CreateBinaryReader(FileStream);
                            else if (Inputfile.FileName.EndsWith(".xlsx"))
                                reader = ExcelReaderFactory.CreateOpenXmlReader(FileStream);
                            else
                                message = "The file format is not supported.";

                            dsexcelRecords = reader.AsDataSet();
                            reader.Close();

                            if (dsexcelRecords != null && dsexcelRecords.Tables.Count > 0)
                            {
                                DataTable dtStudentRecords = dsexcelRecords.Tables[0];

                                var dtCelulares = dsexcelRecords.Tables["Celulares"];
                                var dtChips = dsexcelRecords.Tables["Chips"];
                                var dtServicio = dsexcelRecords.Tables["Servicio"];

                                int output = 0;
                                string localActual = "";
                                for (int j = 4; j <= dtCelulares.Rows.Count-1; j++)
                                {
                                    var cabMeta = new Models.Meta();

                                    cabMeta.LocalId = Convert.ToInt32(dtCelulares.Rows[j][2].ToString().Split('-')[0]);
                                    cabMeta.NumAnio = Convert.ToInt32(dtCelulares.Rows[0][0].ToString().Split(' ')[1]);
                                    decimal metaTotalCe = 0;
                                    decimal cumplimientoTotalCe = 0;
                                    decimal metaTotalCh = 0;
                                    decimal cumplimientoTotalCh = 0;
                                    decimal metaTotalSe = 0;
                                    decimal cumplimientoTotalSe = 0;

                                    localActual = dtCelulares.Rows[j][2].ToString();
                                    
                                    // go through each column in the row
                                    for (int i = 3; i <= 36; i=i+3)
                                    {
                                        var detMeta = new Models.MetaDetalle();
                                        
                                        if (dtCelulares.Rows[j][0] != DBNull.Value)
                                        {
                                            if (dtCelulares.Rows[3][i].ToString() != "%")
                                            {
                                                detMeta.Mes = Convert.ToByte(i/3);
                                                detMeta.MetaCe = Convert.ToDecimal(dtCelulares.Rows[j][i] == DBNull.Value ? "0": dtCelulares.Rows[j][i]);
                                                detMeta.CumplimientoCe = Convert.ToDecimal(dtCelulares.Rows[j][i + 1] == DBNull.Value ? "0" : dtCelulares.Rows[j][i + 1]);
                                                metaTotalCe = Convert.ToDecimal(metaTotalCe + detMeta.MetaCe);
                                                cumplimientoTotalCe = Convert.ToDecimal(cumplimientoTotalCe + detMeta.CumplimientoCe);

                                                IEnumerable<DataRow> dtChipFiltered = dtChips.AsEnumerable()
                                                  .Where(row => row.Field<String>("Column2") == localActual)
                                                  .OrderByDescending(row => row.Field<String>("Column2"));

                                                foreach (var dr in dtChipFiltered)
                                                {                                                    
                                                    detMeta.MetaCh = Convert.ToDecimal(dr[i] == DBNull.Value ? "0" : dr[i]);
                                                    detMeta.CumplimientoCh = Convert.ToDecimal(dr[i + 1] == DBNull.Value ? "0" : dr[i + 1]);
                                                    metaTotalCh = Convert.ToDecimal(metaTotalCh + detMeta.MetaCh);
                                                    cumplimientoTotalCh = Convert.ToDecimal(cumplimientoTotalCh + detMeta.CumplimientoCh);
                                                }                                                
                                                IEnumerable<DataRow> dtServicioFiltered = dtServicio.AsEnumerable()
                                                  .Where(row => row.Field<String>("Column2") == localActual)
                                                  .OrderByDescending(row => row.Field<String>("Column2"));

                                                foreach (var dr in dtServicioFiltered)
                                                {                                                    
                                                    detMeta.MetaSe = Convert.ToDecimal(dr[i] == DBNull.Value ? "0" : dr[i]);
                                                    detMeta.CumplimientoSe = Convert.ToDecimal(dr[i + 1] == DBNull.Value ? "0" : dr[i + 1]);
                                                    metaTotalSe = Convert.ToDecimal(metaTotalSe + detMeta.MetaSe);
                                                    cumplimientoTotalSe = Convert.ToDecimal(cumplimientoTotalSe + detMeta.CumplimientoSe);
                                                }                                                
                                            }
                                        }
                                        cabMeta.MetaDetalle.Add(detMeta);
                                    }
                                    cabMeta.MetaTotalCe = metaTotalCe;
                                    cabMeta.CumplimientoTotalCe = cumplimientoTotalCe;
                                    cabMeta.MetaTotalCh = metaTotalCh;
                                    cabMeta.CumplimientoTotalCh = cumplimientoTotalCh;
                                    cabMeta.MetaTotalSe = metaTotalSe;
                                    cabMeta.CumplimientoTotalSe = cumplimientoTotalSe;

                                    objEntity.Meta.Add(cabMeta);
                                    output = objEntity.SaveChanges();
                                }

                                if (output > 0)
                                    message = "The Excel file has been successfully uploaded.";
                                else
                                    message = "Something Went Wrong!, The Excel file uploaded has fiald.";
                            }
                            else
                                message = "Selected file is empty.";
                        }
                        else
                            message = "Invalid File.";
                    }
                    else
                        ResponseMessage = Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                return message;
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

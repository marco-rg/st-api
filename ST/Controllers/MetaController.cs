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
    [EnableCors(origins: "http://localhost:4200 , https://ctec.sydfast.com , http://ctec.sydfast.com", headers: "*", methods: "*")]
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
                                var dtMicas = dsexcelRecords.Tables["Micas"];
                                var headerRow = dtCelulares.Rows[3];//In row 3 is the table header of the spreadsheet
                                int output = 0;
                                string localActual = "";
                                for (int j = 9; j <= dtCelulares.Rows.Count-1; j++)//4
                                {
                                    var cabMeta = new Models.Meta();

                                    cabMeta.LocalId = Convert.ToInt32(dtCelulares.Rows[j][2].ToString().Split('-')[0]);
                                    cabMeta.NumAnio = DateTime.Now.Year;//Convert.ToInt32(dtCelulares.Rows[0][0].ToString().Split(' ')[1]);
                                    decimal metaTotalCe = 0;
                                    decimal cumplimientoTotalCe = 0;
                                    decimal metaTotalCh = 0;
                                    decimal cumplimientoTotalCh = 0;
                                    decimal metaTotalSe = 0;
                                    decimal cumplimientoTotalSe = 0;
                                    decimal metaTotalMi = 0;
                                    decimal cumplimientoTotalMi = 0;

                                    localActual = dtCelulares.Rows[j][2].ToString();
                                    
                                    // go through each column in the row
                                    for (int i = 3; i <= 12; i=i+3)//3 - 36
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
                                                    detMeta.MetaSe = Math.Round(Convert.ToDecimal(dr[i] == DBNull.Value ? "0" : dr[i]), 2);
                                                    detMeta.CumplimientoSe = Math.Round(Convert.ToDecimal(dr[i + 1] == DBNull.Value ? "0" : dr[i + 1]), 2);
                                                    metaTotalSe = Convert.ToDecimal(metaTotalSe + detMeta.MetaSe);
                                                    cumplimientoTotalSe = Convert.ToDecimal(cumplimientoTotalSe + detMeta.CumplimientoSe);
                                                }  
                                                
                                                //
                                                IEnumerable<DataRow> dtMicaFiltered = dtMicas.AsEnumerable()
                                                    .Where(row => row.Field<String>("Column2") == localActual)
                                                    .OrderByDescending(row => row.Field<String>("Column2"));

                                                foreach (var dr in dtMicaFiltered)
                                                {                                                    
                                                    detMeta.MetaMi = Math.Round(Convert.ToDecimal(dr[i] == DBNull.Value ? "0" : dr[i]), 2);
                                                    detMeta.CumplimientoMi = Math.Round(Convert.ToDecimal(dr[i + 1] == DBNull.Value ? "0" : dr[i + 1]), 2);
                                                    metaTotalMi = Convert.ToDecimal(metaTotalMi + detMeta.MetaMi);
                                                    cumplimientoTotalMi = Convert.ToDecimal(cumplimientoTotalMi + detMeta.CumplimientoMi);
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
                                    cabMeta.MetaTotalMi = metaTotalMi;
                                    cabMeta.CumplimientoTotalMi = cumplimientoTotalMi;
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

        [Route("ReadFile2")]
        [HttpPost]
        public IHttpActionResult ReadFile2()
        {
            /*
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count == 0)
                return BadRequest("No file uploaded.");

            var file = httpRequest.Files[0];
            if (file == null || file.InputStream == null)
                return BadRequest("Invalid file.");

            if (!file.FileName.EndsWith(".xls") && !file.FileName.EndsWith(".xlsx"))
                return BadRequest("The file format is not supported.");*/
            /**/
            string filePath = @"C:\Intel\Metas 2024.xlsx"; // Ruta del archivo
            string message = "";
            DataSet dsexcelRecords = new DataSet();            

            FileStream file = File.Open(filePath, FileMode.Open, FileAccess.Read);


            try
            {
                using (var reader = CreateExcelDataReader(file, file.Name))
                {
                    var dataSet = reader.AsDataSet();
                    reader.Close();

                    if (dataSet == null || dataSet.Tables.Count == 0)
                        return BadRequest("Selected file is empty.");

                    using (var dbContext = new Models.ModelHealthAdvisor())
                    {
                        ProcessDataSet(dataSet, dbContext);
                    }

                    return Ok("The Excel file has been successfully uploaded.");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private IExcelDataReader CreateExcelDataReader(Stream fileStream, string fileName)
        {
            if (fileName.EndsWith(".xls"))
                return ExcelReaderFactory.CreateBinaryReader(fileStream);
            else
                return ExcelReaderFactory.CreateOpenXmlReader(fileStream);
        }

        private void ProcessDataSet(DataSet dataSet, Models.ModelHealthAdvisor dbContext)
        {
            var dtCelulares = dataSet.Tables["Celulares"];
            var dtChips = dataSet.Tables["Chips"];
            var dtServicio = dataSet.Tables["Servicio"];
            var dtMicas = dataSet.Tables["Micas"];
            var rowHeader = dtCelulares.Rows[3];
            for (int rowIndex = 9; rowIndex <= dtCelulares.Rows.Count - 1; rowIndex++)
            {
                var row = dtCelulares.Rows[rowIndex];
                if (row[0].ToString()=="")
                {                    
                    break;
                }
                var meta = CreateMetaFromRow(row, dtChips, dtServicio, dtMicas, rowHeader);
                dbContext.Meta.Add(meta);
            }

            dbContext.SaveChanges();
        }

        private Models.Meta CreateMetaFromRow(DataRow row, DataTable dtChips, DataTable dtServicio, DataTable dtMicas, DataRow rowHeader)
        {
            var meta = new Models.Meta
            {
                LocalId = Convert.ToInt32(row[2].ToString().Split('-')[0]),
                NumAnio = DateTime.Now.Year
            };

            meta.MetaDetalle = ExtractMetaDetails(row, dtChips, dtServicio, dtMicas, rowHeader);
            CalculateTotals(meta);

            return meta;
        }

        private List<Models.MetaDetalle> ExtractMetaDetails(DataRow row, DataTable dtChips, DataTable dtServicio, DataTable dtMicas, DataRow rowHeader)
        {
            var localActual = row[2].ToString();
            var details = new List<Models.MetaDetalle>();
            for (int i = 3; i <= 18; i += 3)
            {
                if (row[0] != DBNull.Value && rowHeader[i].ToString() != "%")//TODO:
                {
                    if (row[i] == DBNull.Value)
                    {

                    }
                    var detail = new Models.MetaDetalle
                    {
                        Mes = Convert.ToByte(i / 3),
                        
                        MetaCe = Convert.ToDecimal((row[i]==DBNull.Value)? 0: row[i] ?? 0),
                        CumplimientoCe = Convert.ToDecimal((row[i + 1] == DBNull.Value)? 0 : row[i + 1] ?? 0)


                    };
                    IEnumerable<DataRow> dtChipFiltered = dtChips.AsEnumerable()
                                                  .Where(fila => fila.Field<String>("Column2") == localActual)
                                                  .OrderByDescending(fila => fila.Field<String>("Column2"));

                    
                    foreach (var dr in dtChipFiltered)
                    {
                        detail.MetaCh = Convert.ToDecimal(dr[i] == DBNull.Value ? "0" : dr[i]);
                        detail.CumplimientoCh = Convert.ToDecimal(dr[i + 1] == DBNull.Value ? "0" : dr[i + 1]);
                        
                    }
                    IEnumerable<DataRow> dtServicioFiltered = dtServicio.AsEnumerable()
                      .Where(fila => fila.Field<String>("Column2") == localActual)
                      .OrderByDescending(fila => fila.Field<String>("Column2"));

                    foreach (var dr in dtServicioFiltered)
                    {
                        detail.MetaSe = Math.Round(Convert.ToDecimal(dr[i] == DBNull.Value ? "0" : dr[i]), 2);
                        detail.CumplimientoSe = Math.Round(Convert.ToDecimal(dr[i + 1] == DBNull.Value ? "0" : dr[i + 1]), 2);
                        
                    }

                    //
                    IEnumerable<DataRow> dtMicaFiltered = dtMicas.AsEnumerable()
                        .Where(fila => fila.Field<String>("Column2") == localActual)
                        .OrderByDescending(fila => fila.Field<String>("Column2"));

                    foreach (var dr in dtMicaFiltered)
                    {
                        detail.MetaMi = Math.Round(Convert.ToDecimal(dr[i] == DBNull.Value ? "0" : dr[i]), 2);
                        detail.CumplimientoMi = Math.Round(Convert.ToDecimal(dr[i + 1] == DBNull.Value ? "0" : dr[i + 1]), 2);
                        
                    }
                    
                    details.Add(detail);
                }
            }
            return details;
        }

        private void CalculateTotals(Models.Meta meta)
        {
            foreach (var detail in meta.MetaDetalle)
            {
                meta.MetaTotalCe = Convert.ToDecimal(meta.MetaTotalCe + detail.MetaCe);// += detail.MetaCe;
                meta.CumplimientoTotalCe = Convert.ToDecimal(meta.CumplimientoTotalCe + detail.CumplimientoCe);//+= detail.CumplimientoCe;
                // Repeat for other fields
                meta.MetaTotalCh = Convert.ToDecimal(meta.MetaTotalCh + detail.MetaCh);
                meta.CumplimientoTotalCh = Convert.ToDecimal(meta.CumplimientoTotalCh + detail.CumplimientoCh);

                meta.MetaTotalSe = Convert.ToDecimal(meta.MetaTotalSe + detail.MetaSe);
                meta.CumplimientoTotalSe = Convert.ToDecimal(meta.CumplimientoTotalSe + detail.CumplimientoSe);

                meta.MetaTotalMi = Convert.ToDecimal(meta.MetaTotalMi + detail.MetaMi);
                meta.CumplimientoTotalMi = Convert.ToDecimal(meta.CumplimientoTotalMi + detail.CumplimientoMi);
            }
        }
    }
}

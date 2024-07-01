using ST.Controllers;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ST.Models
{
    [EnableCors(origins: "http://localhost:4200 , https://ctec.sydfast.com , http://ctec.sydfast.com", headers: "*", methods: "*")]    
    [RoutePrefix("api/Encuestas")]
    public class EncuestasController : ApiController
    {
        // GET api/<controller>
        public Models.Encuestas objEncuestas = null;
        //   public Models.ModelHealthAdvisor dbContext = new Models.ModelHealthAdvisor();
        [HttpGet]
        [Route("OldAll")]
        public Models.Results GetAll()
        {
            Models.Results resultado = new Models.Results();
            using (var dbContextPathological = new Models.ModelHealthAdvisor())
            {

                var username = TokenGenerator.GetUserSystem(Request);
                var usuario = dbContextPathological.UserSystem.Where(t => t.usrLogon.Equals(username)).FirstOrDefault();
                if (usuario != null)
                {
                    string[] listCompanys = usuario.usrCompanyList.Split('|');
                    var listCompanyIds = listCompanys.Select(x => long.Parse(x)).ToList();

                    //resultado.OBJETO = dbContextPathological.Encuestas
                    //                    .Include(b => b.EncuestasDetalle.)
                    //                    .ToList();
                    resultado.OBJETO = dbContextPathological.Encuestas
                                      .Include("EncuestasDetalle")
                                      .Include("LocalesNacionales")
                                      /*.Include("EncuestasDetalle.Images")
                                      
                                      .Include("Site.SkrettingSector")
                                      .Where(z => z.EstaEliminado != true && listCompanyIds.Contains(z.Site.Company.CompAquasymId))*/
                                      .Select(X => new
                                      {
                                          X.EncuestaId,
                                          X.LocalesNacionales.NombreLocal,
                                          X.LocalesNacionales.FormatoLocal,
                                          X.EncargadoId,
                                          X.CalificacionMarca,
                                         X.CalificacionTelefono
                                      }
                                          )
                                      .ToList();

                }
                resultado.MENSAJE = "Listado de Análisis Patológicos consultadas exitosamente";
                resultado.STATUS = "success";
                return resultado;

            }

        }

        [HttpPost]
        [Route("All")]
        public Models.Results PostAll([FromBody]Models.IF_PDI_LIST _objIfPDI)
        {
            Models.Results resultado = new Models.Results();
            /*if (TokenGenerator.HasPermissions(Request, "HA_PATHOLOGICAL_ANALYSIS", "Create"))
            {*/

                using (var dbContextPathological = new Models.ModelHealthAdvisor())
                {
                    if (_objIfPDI == null)
                    {
                        _objIfPDI = new Models.IF_PDI_LIST();
                        _objIfPDI._dateStart = null;
                        _objIfPDI._dateEnd = null;
                    }


                    var username = TokenGenerator.GetUserSystem(Request);
                    var usuario = dbContextPathological.UserExternal.Where(t => t.Username.Equals(username)).FirstOrDefault();
                    if (usuario != null)
                    {
                        /*string[] listCompanys = usuario.usrCompanyList.Split('|');
                        var listCompanyIds = listCompanys.Select(x => long.Parse(x)).ToList();*/

                        //resultado.OBJETO = dbContextPathological.Encuestas
                        //                    .Include(b => b.EncuestasDetalle.)
                        //                    .ToList();
                        var consulta = dbContextPathological.Encuestas
                                      .Include("EncuestasDetalle")
                                      .Include("LocalesNacionales")
                                      .Where(z => z.EstaEliminado != true)
                                          .Select(X => new
                                          {
                                              X.EncuestaId,
                                              X.LocalesNacionales.NombreLocal,
                                              X.LocalesNacionales.FormatoLocal,
                                              X.EncargadoId,
                                              X.CalificacionMarca,
                                              X.CalificacionTelefono,
                                              X.CreadoAl
                                          }
                                              ).OrderByDescending(j => j.EncuestaId); //.ToList();

                        if (_objIfPDI._dateStart != null && _objIfPDI._dateEnd != null)
                        {
                            if (_objIfPDI._dateEnd >= _objIfPDI._dateStart)
                                consulta = consulta.Where(x => x.CreadoAl >= _objIfPDI._dateStart
                                                      && x.CreadoAl <= _objIfPDI._dateEnd).OrderByDescending(j => j.EncuestaId);
                        }

                        resultado.OBJETO = consulta.ToList();
                        resultado.MENSAJE = "Listado de Encuestas consultadas exitosamente";
                        resultado.STATUS = "success";
                        return resultado;

                    }
                    resultado.OBJETO = null;
                    resultado.MENSAJE = "No data";
                    resultado.STATUS = "error";
                    return resultado;

                }

            /*}
            else
            {
                resultado.OBJETO = null;
                resultado.MENSAJE = "No authorized";
                resultado.STATUS = "unsuccess";
                return resultado;



            }*/

        }

        /*
        [HttpGet]
        [Route("PathologicalEntities")]
        public Models.Results PathologicalEntities()
        {
            Models.Results resultado = new Models.Results();
            //if (TokenGenerator.HasPermissions(Request, "IC_ALARM_RESPONSABLE", "Read"))
            //{
            using (var _context = new Models.ModelHealthAdvisor())
            {
                var username = TokenGenerator.GetUserSystem(Request);
                var usuario = _context.UserSystem.Where(t => t.usrLogon.Equals(username)).FirstOrDefault();
                if (usuario != null)
                {
                    string[] listCompanys = usuario.usrCompanyList.Split('|');
                    var listCompanyIds = listCompanys.Select(x => long.Parse(x)).ToList();


                    var listado = _context.Company
                        .Include("Site")
                        .Include("Site.Unit")
                        .Include("Site.SampleLocationInstance")
                        .Where(x => listCompanyIds.Contains(x.CompAquasymId))

                         .Select(X => new
                         {
                             X.CompAquasymId,
                             X.cmpName,
                             Site = X.Site.Select(y => new
                             {
                                 y.SitAquasymId,
                                 y.sitName,
                                 SkrettingSectorId = _context.SkrettingSector.Where(z => z.skseId == y.sitSkrettingSectorId).Select(w => (long?)w.skseId).FirstOrDefault(),
                                 SkrettingSectorName = _context.SkrettingSector.Where(z => z.skseId == y.sitSkrettingSectorId).Select(w => w.skseName).FirstOrDefault(),

                                 //Unit = y.Unit.Where(t => t.ProductionGroup.Any(p => p.fgIsClosed == false))
                                 //                                             .Select(z => new { z.UniAquasimId, z.uniName }),
                                 Unit = y.Unit.Where(t => t.UniHasPGOpened == true && t.IsDeleted != true)
                                 //Unit = y.Unit.Where(t => t.IsDeleted != true)    
                                                                              .Select(z => new {
                                                                                  z.UniAquasimId,
                                                                                  z.uniName,

                                                                                  uniActive = z.UniHasPGOpened.HasValue ? z.UniHasPGOpened : false,
                                                                              }),

                                 SampleLocationInstance = y.SampleLocationInstance.Select(t => new { t.slinId, t.slinKey, t.slinName, t.slinDescription })
                             })
                         })

                        .ToList();

                    resultado.OBJETO = listado;
                    resultado.MENSAJE = "";
                    resultado.STATUS = "success";
                }
            }
            return resultado;
        }
        */
        
        [HttpGet]
        [Route("Get/{id:int}")]
        public Models.Results Get(long id)
        {
            using (var dbContextPathological = new Models.ModelHealthAdvisor())
            {

                Models.Results resultado = new Models.Results();
                //Models.Encuestas objEncuestas = null;
                var objEncuestasResult = dbContextPathological.Encuestas
                    .Include("EncuestasDetalle")
                    //.Include("EncuestasDetalle.Images")
                    .Include("LocalesNacionales")
                    .Where(p => p.EncuestaId == id)
                    .Select(X => new
                    {
                        X.EncuestaId,
                        X.CodigoLocal,
                        X.CreadoAl,
                        X.LocalesNacionales.NombreLocal,
                        X.Observacion,
                        X.EncargadoId,
                        X.CalificacionMarca,
                        X.CalificacionTelefono,
                        //TechnicalId = X.UserSystem.usrId,
                        //TechnicalName = X.UserSystem.usrFirstName + " " + X.UserSystem.usrLastName,
                        X.EstaEliminado,
                        /**/
                        EncuestasDetalle = X.EncuestasDetalle.GroupBy(u => new { u.Pregunta.CategoriaId, u.Pregunta.Categorias.Descripcion })
                        .Select(P => new {
                            CategoriaId = P.FirstOrDefault().Pregunta.CategoriaId,
                            P.FirstOrDefault().Pregunta.Categorias.Descripcion,
                            Preguntas = X.EncuestasDetalle.Select(d => new {
                                d.EncuestaId,
                                d.EncuestaDetalleId,
                                d.PreguntaId,
                                d.Peso,
                                d.Puntaje,
                                d.Resultado,
                                d.Porcentaje,
                                d.Comentario,
                                d.Adjunto,
                                CategoriaId = d.Pregunta.CategoriaId
                            }).Where(y => y.CategoriaId == P.FirstOrDefault().Pregunta.CategoriaId).ToList()
                        })
                         .ToList(),
                        IndicadoresDetalle = dbContextPathological.MetaDetalle//.Include("Meta")
                         .Where(t => t.PorcentajeCe > 0 && t.PorcentajeCh > 0 && t.PorcentajeSe > 0 && t.Meta.LocalId == X.CodigoLocal)
                         .Select(y => new
                         {
                             //y.Meta.LocalId,
                             Mes= y.Mes,//string.Format("{0} {1}", "Month", y.Mes),//
                             y.PorcentajeCe,
                             y.PorcentajeCh,
                             y.PorcentajeSe
                         }).OrderByDescending(c => c.Mes).Take(3).ToList()
                             /*,
                            EncuestasDetalle = X.EncuestasDetalle.Select(P => new
                            {                            
                                CategoriaId = P.Pregunta.CategoriaId,
                                Descripcion = P.Pregunta.Categorias.Descripcion
                            }).GroupBy(u => new { u.CategoriaId, u.Descripcion }).FirstOrDefault().ToList()*/
                         }).ToList();

                if (objEncuestasResult != null)
                {
                    resultado.OBJETO = objEncuestasResult;
                    resultado.MENSAJE = "Análisis Patológico consultado exitosamente";
                    resultado.STATUS = "success";
                    return resultado;
                }
                else
                {

                    resultado.MENSAJE = "No existe Análisis Patológico con el identificador indicado";
                    resultado.STATUS = "error";
                    return resultado;
                }
            }
        }
        
        // POST api/<controller>
        /*
        [HttpPost]
        [Route("Create")]
        public Models.Results Post([FromBody]Models.Encuestas _objEncuestas)
        {
            using (var dbContextPathological = new Models.ModelHealthAdvisor())
            {
                Models.Results resultado = new Models.Results();
                try
                {

                    if (ModelState.IsValid)
                    {

                        if (_objEncuestas.EncuestasDetalle.Count > 0)
                        {

                            objEncuestas = _objEncuestas;

                            //obtener el PATH DEL SERVIDOR a donde se alojaran las imagenes
                            string filePath = "";


                            var objSystemParameter = dbContextPathological.SystemParameter.Where(x => x.field == "PathServerFolder").FirstOrDefault();
                            filePath = objSystemParameter.value;


                            //filePath =  System.Web.HttpContext.Current.Server.MapPath("~/ImageStorage");

                            foreach (Models.EncuestasDetalle item in _objEncuestas.EncuestasDetalle)
                            {
                                foreach (Models.Images imagen in item.Images)
                                {
                                    if (!System.IO.Directory.Exists(@filePath))
                                    {
                                        System.IO.Directory.CreateDirectory(@filePath);
                                    }

                                    byte[] imageBytes = Convert.FromBase64String(imagen.imImageBase64String);
                                    System.IO.File.WriteAllBytes(System.IO.Path.Combine(@filePath, imagen.imImageFileName), imageBytes);
                                }

                            }




                            dbContextPathological.Encuestas.Add(_objEncuestas);
                            dbContextPathological.SaveChanges();
                            resultado.OBJETO = objEncuestas;
                            resultado.MENSAJE = "Pathological Data Input created.";
                            resultado.STATUS = "success";
                            return resultado;
                        }
                        else
                        {

                            resultado.MENSAJE = "No exist details.";
                            resultado.STATUS = "error";
                            return resultado;
                        }
                    }
                    else
                    {
                        // ModelState.
                        string mensajeDeErrores = "";
                        foreach (var state in ModelState)
                        {
                            foreach (var error in state.Value.Errors)
                            {
                                mensajeDeErrores = mensajeDeErrores + error.ErrorMessage + "." + System.Environment.NewLine;
                            }
                        }

                        resultado.MENSAJE = mensajeDeErrores;
                        resultado.STATUS = "error";
                        return resultado;

                    }

                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {

                    var sqlex = ex.InnerException.InnerException as System.Data.SqlClient.SqlException;

                    if (sqlex != null)
                    {
                        switch (sqlex.Number)
                        {
                            case 547:
                                resultado.MENSAJE = "Pathological Data Input cant be created";
                                break; //FK exception
                            case 2601:
                                resultado.MENSAJE = "Pathological Data Input has same values.";//primary key exception
                                break;
                            case 2627:
                            default: resultado.MENSAJE = ex.Message; break;//otra excepcion que no controlo.
                        }
                    }
                    resultado.STATUS = "error";
                    return resultado;
                }
                catch (System.IO.IOException ex)
                {


                    resultado.STATUS = ex.Message;
                    return resultado;
                }
            }


        }
        */
        /*
        [HttpPost]
        [Route("SaveHeader")]
        public Models.Results SaveHeader([FromBody]Models.Encuestas _objEncuestas)
        {
            using (var dbContextPathological = new Models.ModelHealthAdvisor())
            {
                Models.Results resultado = new Models.Results();
                try
                {
                    _objEncuestas.pdiSiteDateTime = new Models.Utilities().convertToTimeZoneEcuador(DateTime.Now);

                    string userName = TokenGenerator.GetUserSystem(Request);
                    var user = dbContextPathological.UserSystem.FirstOrDefault(p => p.usrLogon.Equals(userName));

                    _objEncuestas.pdiTechnicalUserId = user.usrId;
                    _objEncuestas.pdiCreatorUserId = user.usrLogon;
                    _objEncuestas.pdiCreationTime = new Models.Utilities().convertToTimeZoneEcuador(DateTime.Now);
                    _objEncuestas.pdiLastModificationTime = null;
                    _objEncuestas.pdiLastModificationUserId = null;
                    _objEncuestas.pdiDeleterUserId = null;
                    _objEncuestas.pdiDeletionTime = null;
                    _objEncuestas.pdiIsDeleted = null;



                    if (ModelState.IsValid)
                    {

                        objEncuestas = _objEncuestas;

                        dbContextPathological.Encuestas.Add(_objEncuestas);
                        dbContextPathological.SaveChanges();
                        resultado.OBJETO = objEncuestas;
                        resultado.MENSAJE = "Pathological Data Input created.";
                        resultado.STATUS = "success";
                        return resultado;

                    }
                    else
                    {
                        // ModelState.
                        string mensajeDeErrores = "";
                        foreach (var state in ModelState)
                        {
                            foreach (var error in state.Value.Errors)
                            {
                                mensajeDeErrores = mensajeDeErrores + (String.IsNullOrEmpty(error.ErrorMessage) ? error.Exception.Message : error.ErrorMessage) + "." + System.Environment.NewLine;
                            }
                        }

                        resultado.MENSAJE = mensajeDeErrores;
                        resultado.STATUS = "error";
                        return resultado;

                    }

                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {

                    var sqlex = ex.InnerException.InnerException as System.Data.SqlClient.SqlException;

                    if (sqlex != null)
                    {
                        switch (sqlex.Number)
                        {
                            case 547:
                                resultado.MENSAJE = "Pathological Data Input cant be created";
                                break; //FK exception
                            case 2601:
                                resultado.MENSAJE = "Pathological Data Input has same values.";//primary key exception
                                break;
                            case 2627:
                            default: resultado.MENSAJE = ex.Message; break;//otra excepcion que no controlo.
                        }
                    }
                    resultado.STATUS = "error";
                    return resultado;
                }
                catch (System.IO.IOException ex)
                {


                    resultado.STATUS = ex.Message;
                    return resultado;
                }
            }


        }
        */
        
        [HttpPost]
        [Route("SaveHeaderDetail")]
        public Models.Results SaveHeaderDetail([FromBody]Models.Encuestas _objEncuestas)
        {
            using (var dbContextPathological = new Models.ModelHealthAdvisor())
            {
                Models.Results resultado = new Models.Results();
                try
                {
                    _objEncuestas.CreadoAl = new Models.Utilities().convertToTimeZoneEcuador(DateTime.Now);

                    string userName = TokenGenerator.GetUserSystem(Request);
                    var user = dbContextPathological.UserExternal.FirstOrDefault(p => p.Username.Equals(userName));

                    //_objEncuestas.pdiTechnicalUserId = user.usrId;
                    _objEncuestas.UserCreatorId = user.Username;
                    _objEncuestas.CreadoAl = new Models.Utilities().convertToTimeZoneEcuador(DateTime.Now);
                    _objEncuestas.ModificadoAl = null;
                    _objEncuestas.UserModifierId = null;
                    _objEncuestas.UserDeleterId = null;
                    _objEncuestas.EliminadoAl = null;
                    _objEncuestas.EstaEliminado = null;

                    //_objEncuestas.pdiStatus = 1;

                    if (ModelState.IsValid)
                    {

                        //objEncuestas = _objEncuestas;

                        dbContextPathological.Encuestas.Add(_objEncuestas);
                        //Encuestas Details
                        var total = 0;

                        foreach (var _objEncuestasDetalle in _objEncuestas.EncuestasDetalle)
                        {
                            /*if (_objEncuestasDetalle.pdidShrimpQty == null)
                            {
                                _objEncuestasDetalle.pdidShrimpQty = 0;
                                _objEncuestasDetalle.pdidDays = 0;
                                _objEncuestasDetalle.pdidWeight = 0;
                            }
                            if (_objEncuestasDetalle.pdidDays == null)
                            {
                                _objEncuestasDetalle.pdidShrimpQty = 0;
                                _objEncuestasDetalle.pdidDays = 0;
                                _objEncuestasDetalle.pdidWeight = 0;
                            }
                            if (_objEncuestasDetalle.pdidWeight == null)
                            {
                                _objEncuestasDetalle.pdidShrimpQty = 0;
                                _objEncuestasDetalle.pdidDays = 0;
                                _objEncuestasDetalle.pdidWeight = 0;
                            }
                            if (_objEncuestasDetalle.pdidChromatophoresQty == null)
                            {
                                _objEncuestasDetalle.pdidChromatophoresQty = 0;
                            }
                            if (_objEncuestasDetalle.pdidUnitAquasimId != null)
                            {
                                // Cuando crean piscinas sin producción Group
                                var objPGClosed = dbContextPathological.Unit.Where(u => u.UniAquasimId == _objEncuestasDetalle.pdidUnitAquasimId).Select(u => new { u.UniHasPGOpened }).FirstOrDefault();
                                if (objPGClosed.Equals(true)) // listLaboratory.Count() == 0
                                {
                                    var objPGTmp = dbContextPathological.ProductionGroup.Where(x => x.PGUnitAquasymId == _objEncuestasDetalle.pdidUnitAquasimId).OrderByDescending(a => a.fgID).FirstOrDefault();
                                    _objEncuestasDetalle.pdidProductionGroupAquasimId = objPGTmp.PGAquasimId;
                                }
                                if (_objEncuestasDetalle.pdidProductionGroupAquasimId.HasValue)
                                {
                                    _objEncuestasDetalle.pdidProductionGroupAquasimId = null;
                                }
                            }
                            else
                            {
                                //validacion para guardar datos de precrias
                                _objEncuestasDetalle.pdidUnitAquasimId = null;
                                _objEncuestasDetalle.pdidProductionGroupAquasimId = null;
                            }*/
                            //if (_objEncuestasDetalle.pdidId != null)
                            //{
                            /*_objEncuestasDetalle.Images.Select(c => { c.imCreationTime = DateTime.Now; return c; }).ToList();
                            _objEncuestasDetalle.Images.Select(c => { c.imCreatorUserId = TokenGenerator.GetUserSystem(Request); return c; }).ToList();
            */
                            //find header                            
                            objEncuestas = _objEncuestas;
                            //Pregunta-Categoria
                            var objPregCat = objEncuestas.EncuestasDetalle.Join(dbContextPathological.Pregunta, enc => enc.PreguntaId, pre => pre.PreguntaId, (enc, pre) => new { CategoriaId = pre.CategoriaId, PreguntaId = enc.PreguntaId }).ToList();
                            var aux = objPregCat.FirstOrDefault(a => a.PreguntaId == _objEncuestasDetalle.PreguntaId);
                            if (aux.CategoriaId==1)
                            {
                                _objEncuestasDetalle.Resultado = _objEncuestasDetalle.Peso * _objEncuestasDetalle.Puntaje;
                            }
                            else
                            {
                                var esCorrecta = dbContextPathological.PreguntaDetalle.FirstOrDefault(n => n.PreguntaDetalleId == _objEncuestasDetalle.Puntaje).EsCorrecta;
                                if (esCorrecta)
                                {
                                    _objEncuestasDetalle.Resultado = _objEncuestasDetalle.Peso * 1;
                                }
                                else
                                {
                                    _objEncuestasDetalle.Resultado = _objEncuestasDetalle.Peso * 0;
                                }                                
                            }                            
                            total = total + (_objEncuestasDetalle.Peso * _objEncuestasDetalle.Puntaje);
                            if (objEncuestas != null)
                            {
                                /*dbContextPathological.EncuestasDetalle.Add(_objEncuestasDetalle);
                                
                                if (objEncuestas.pdiItemTotals == objEncuestas.pdiItemsSaved)
                                {
                                    objEncuestas.pdiStatus = 1;
                                    //PonerAqui el Delete Fisico si ya ha guardado los registros correctamente
                                }*/
                            }
                            else
                            {                                
                            }
                        }
                        var result = objEncuestas.EncuestasDetalle.Join(dbContextPathological.Pregunta, enc=> enc.PreguntaId, pre=>pre.PreguntaId,(enc, pre) => new { CategoriaId = pre.CategoriaId, Resultado = enc.Resultado, Peso = enc.Peso})
                            .GroupBy(l => l.CategoriaId)
                            .Select(cl => new //ResultLine
                            {
                                CategoriaId = cl.First().CategoriaId,
                                //Quantity = cl.Count().ToString(),
                                Subtotal = cl.Sum(c => c.Resultado),
                                SubPeso = cl.Sum(c=> c.Peso)
                            }).ToList();
                        var objJoin = objEncuestas.EncuestasDetalle.Join(dbContextPathological.Pregunta, enc => enc.PreguntaId, pre => pre.PreguntaId, (enc, pre) => new { CategoriaId = pre.CategoriaId, PreguntaId = enc.PreguntaId }).ToList();
                        foreach (var _objEncuestasDetalle in _objEncuestas.EncuestasDetalle)
                        {
                            var aux = objJoin.FirstOrDefault(a => a.PreguntaId == _objEncuestasDetalle.PreguntaId);
                            var totalGrupo = result.FirstOrDefault(b => b.CategoriaId == aux.CategoriaId);
                            if (aux.CategoriaId==1)
                            {
                                _objEncuestasDetalle.Porcentaje = ((decimal)_objEncuestasDetalle.Resultado / (decimal)totalGrupo.Subtotal) * 100m;
                            }
                            else
                            {
                                _objEncuestasDetalle.Porcentaje = ((decimal)_objEncuestasDetalle.Resultado / (decimal)totalGrupo.SubPeso) * 100m;
                            }                            
                        }
                        var resultGroup = objEncuestas.EncuestasDetalle.Join(dbContextPathological.Pregunta, enc => enc.PreguntaId, pre => pre.PreguntaId, (enc, pre) => new { CategoriaId = pre.CategoriaId, Porcentaje = enc.Porcentaje })
                            .GroupBy(l => l.CategoriaId)
                            .Select(cl => new //ResultLine
                            {
                                CategoriaId = cl.First().CategoriaId,
                                //Quantity = cl.Count().ToString(),
                                Subtotal = cl.Sum(c => c.Porcentaje),
                            }).ToList();
                        objEncuestas.CalificacionMarca = resultGroup.FirstOrDefault().Subtotal;//Calificación Local
                        objEncuestas.CalificacionTelefono = resultGroup.LastOrDefault().Subtotal;

                        dbContextPathological.SaveChanges();
                        resultado.OBJETO = objEncuestas;
                        resultado.MENSAJE = "Pathological Data Input created.";
                        resultado.STATUS = "success";
                        return resultado;

                    }
                    else
                    {
                        // ModelState.
                        string mensajeDeErrores = "";
                        foreach (var state in ModelState)
                        {
                            foreach (var error in state.Value.Errors)
                            {
                                mensajeDeErrores = mensajeDeErrores + (String.IsNullOrEmpty(error.ErrorMessage) ? error.Exception.Message : error.ErrorMessage) + "." + System.Environment.NewLine;
                            }
                        }

                        resultado.MENSAJE = mensajeDeErrores;
                        resultado.STATUS = "error";
                        return resultado;

                    }

                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {

                    var sqlex = ex.InnerException.InnerException as System.Data.SqlClient.SqlException;

                    if (sqlex != null)
                    {
                        switch (sqlex.Number)
                        {
                            case 547:
                                resultado.MENSAJE = "Pathological Data Input cant be created";
                                break; //FK exception
                            case 2601:
                                resultado.MENSAJE = "Pathological Data Input has same values.";//primary key exception
                                break;
                            case 2627:
                            default: resultado.MENSAJE = ex.Message; break;//otra excepcion que no controlo.
                        }
                    }
                    resultado.STATUS = "error";
                    return resultado;
                }
                catch (System.IO.IOException ex)
                {


                    resultado.STATUS = ex.Message;
                    return resultado;
                }
            }


        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_objEncuestasDetalle"></param>
        /// <returns></returns>
        /*
        [HttpPost]
        [Route("SaveDetail")]
        public Models.Results SaveDetail([FromBody]Models.EncuestasDetalle _objEncuestasDetalle)
        {

            using (var dbContextPathological = new Models.ModelHealthAdvisor())
            {
                Models.Results resultado = new Models.Results();

                try
                {
                    //JsonConvert.SerializeObject(yourObject);


                    if (ModelState.IsValid)
                    {

                        //if (_objEncuestasDetalle.pdidShrimpQty == null){
                        //      _objEncuestasDetalle.pdidShrimpQty = 0;
                        //      _objEncuestasDetalle.pdidDays = 0;
                        //      _objEncuestasDetalle.pdidWeight = 0;

                        //}


                        //if (_objEncuestasDetalle.pdidDays == null){
                        //    _objEncuestasDetalle.pdidShrimpQty = 0;
                        //    _objEncuestasDetalle.pdidDays = 0;
                        //    _objEncuestasDetalle.pdidWeight = 0;

                        //}

                        //if (_objEncuestasDetalle.pdidWeight == null){
                        //    _objEncuestasDetalle.pdidShrimpQty = 0;
                        //    _objEncuestasDetalle.pdidDays = 0;
                        //    _objEncuestasDetalle.pdidWeight = 0;

                        //}

                        if (_objEncuestasDetalle.pdidChromatophoresQty == null)
                        {
                            _objEncuestasDetalle.pdidChromatophoresQty = 0;
                        }



                        if (_objEncuestasDetalle.pdidUnitAquasimId != null)
                        {
                            

                            // Cuando crean piscinas sin producción Group

                            var objPGClosed = dbContextPathological.Unit.Where(u => u.UniAquasimId == _objEncuestasDetalle.pdidUnitAquasimId).Select(u => new { u.UniHasPGOpened }).FirstOrDefault();

                            if (objPGClosed.Equals(true)) // listLaboratory.Count() == 0
                            {
                                var objPGTmp = dbContextPathological.ProductionGroup.Where(x => x.PGUnitAquasymId == _objEncuestasDetalle.pdidUnitAquasimId).OrderByDescending(a => a.fgID).FirstOrDefault();
                                _objEncuestasDetalle.pdidProductionGroupAquasimId = objPGTmp.PGAquasimId;

                            }



                            if (_objEncuestasDetalle.pdidProductionGroupAquasimId.HasValue)
                            {
                                _objEncuestasDetalle.pdidProductionGroupAquasimId = null;
                            }


                        }
                        else
                        {
                            //validacion para guardar datos de precrias

                            _objEncuestasDetalle.pdidUnitAquasimId = null;
                            _objEncuestasDetalle.pdidProductionGroupAquasimId = null;

                        }


                        //if (_objEncuestasDetalle.pdidId != null)
                        //{
                        _objEncuestasDetalle.Images.Select(c => { c.imCreationTime = DateTime.Now; return c; }).ToList();
                        _objEncuestasDetalle.Images.Select(c => { c.imCreatorUserId = TokenGenerator.GetUserSystem(Request); return c; }).ToList();

                        //find header
                        var objEncuestas = dbContextPathological.Encuestas
                            .Where(x => x.pdiId == _objEncuestasDetalle.pdidEncuestasId).FirstOrDefault();
                        if (objEncuestas != null)
                        {

                            dbContextPathological.EncuestasDetalle.Add(_objEncuestasDetalle);

                            objEncuestas.pdiItemsSaved = objEncuestas.pdiItemsSaved + 1;

                            if (objEncuestas.pdiItemTotals == objEncuestas.pdiItemsSaved)
                            {
                                objEncuestas.pdiStatus = 1;
                                //PonerAqui el Delete Fisico si ya ha guardado los registros correctamente

                                

                            }
                            dbContextPathological.Encuestas.Attach(objEncuestas);
                            dbContextPathological.Entry(objEncuestas).Property(x => x.pdiItemsSaved).IsModified = true;
                            dbContextPathological.Entry(objEncuestas).Property(x => x.pdiStatus).IsModified = true;



                            dbContextPathological.SaveChanges();
                            resultado.OBJETO = objEncuestas;
                            resultado.MENSAJE = "Pathological Data Input created.";
                            resultado.STATUS = "success";
                            return resultado;

                        }
                        else
                        {
                            resultado.OBJETO = null;
                            resultado.MENSAJE = "Pathological Header no exist.";
                            resultado.STATUS = "error";
                            return resultado;
                        }

                        // }

                        //else
                        //{


                        //}

                    }
                    else
                    {
                        // ModelState.
                        string mensajeDeErrores = "";
                        foreach (var state in ModelState)
                        {
                            foreach (var error in state.Value.Errors)
                            {
                                mensajeDeErrores = mensajeDeErrores + error.ErrorMessage + "." + System.Environment.NewLine;
                            }
                        }

                        resultado.MENSAJE = mensajeDeErrores;
                        resultado.STATUS = "error";
                        return resultado;

                    }

                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {

                    var sqlex = ex.InnerException.InnerException as System.Data.SqlClient.SqlException;

                    //Cuando Hay error al guardar se reverse y no se eliminen los registros

                    var objEncuestasDetalle = (from a in dbContextPathological.EncuestasDetalle where a.pdidEncuestasId == objEncuestas.pdiId select a)
                                               .FirstOrDefault();


                    foreach (var childModel in objEncuestas.EncuestasDetalle)
                    {
                        var existingChild = objEncuestas.EncuestasDetalle
                            .Where(c => c.pdidId == childModel.pdidId && c.pdidIsDeleted == true && c.pdidId != default(int))
                            .FirstOrDefault();


                        if (existingChild != null)
                        {

                            childModel.pdidIsDeleted = false;
                            dbContextPathological.Entry(existingChild).CurrentValues.SetValues(childModel);

                        }


                    }


                    foreach (var childModel in objEncuestasDetalle.Images)
                    {
                        var existingChild = objEncuestas.EncuestasDetalle
                            .Where(c => c.pdidId == childModel.imEncuestasDetalleId && c.pdidIsDeleted == true && c.pdidId != default(int))
                            .FirstOrDefault();



                        if (existingChild != null)
                        {

                            childModel.imIsDeleted = false;
                            dbContextPathological.Entry(existingChild).CurrentValues.SetValues(childModel);
                        }


                    }

                    if (sqlex != null)
                    {
                        switch (sqlex.Number)
                        {
                            case 547:
                                resultado.MENSAJE = "Pathological Data Input cant be created";
                                break; //FK exception
                            case 2601:
                                resultado.MENSAJE = "Pathological Data Input has same values.";//primary key exception
                                break;
                            case 2627:
                            default: resultado.MENSAJE = ex.Message; break;//otra excepcion que no controlo.
                        }
                    }
                    resultado.STATUS = "error";
                    return resultado;
                }
                catch (System.IO.IOException ex)
                {


                    resultado.STATUS = ex.Message;
                    return resultado;
                }
            }


        }
        */

        /*
        [HttpPost]
        [Route("Update")]
        public Models.Results PostUpdate([FromBody]Models.Encuestas _objEncuestas)
        {
            using (var dbContextPathological = new Models.ModelHealthAdvisor())
            {
                Models.Results resultado = new Models.Results();
                try
                {
                    objEncuestas = (from a in dbContextPathological.Encuestas.Include("EncuestasDetalle") where a.EncuestaId == _objEncuestas.EncuestaId select a)
                                                .FirstOrDefault();


                    var objEncuestasDetalle = (from a in dbContextPathological.EncuestasDetalle where a.EncuestaId == _objEncuestas.EncuestaId select a)
                                                .FirstOrDefault();


                    if (objEncuestas != null)
                    {

                        var PathologicaDetailList = objEncuestas.EncuestasDetalle
                                .Select(z => z.EncuestaDetalleId).ToList();

                        //update Items

                        
                        dbContextPathological.EncuestasDetalle.RemoveRange(objEncuestas.EncuestasDetalle);

                        

                        //update header
                        objEncuestas.Observacion = _objEncuestas.Observacion;
                        objEncuestas.LocalId = _objEncuestas.LocalId;
                        objEncuestas.EncargadoId = _objEncuestas.EncargadoId;
                        
                        objEncuestas.ModificadoAl = DateTime.Now;
                        objEncuestas.UserModifierId = TokenGenerator.GetUserSystem(Request);
                        dbContextPathological.Encuestas.Attach(objEncuestas);
                        dbContextPathological.Entry(objEncuestas).Property(x => x.Observacion).IsModified = true;
                        dbContextPathological.Entry(objEncuestas).Property(x => x.LocalId).IsModified = true;
                        
                        dbContextPathological.Entry(objEncuestas).Property(x => x.ModificadoAl).IsModified = true;
                        dbContextPathological.Entry(objEncuestas).Property(x => x.UserModifierId).IsModified = true;
                        dbContextPathological.Entry(objEncuestas).Property(x => x.EncargadoId).IsModified = true;


                        //update detail


                        dbContextPathological.SaveChanges();


                    }
                    resultado.OBJETO = objEncuestas;
                    resultado.MENSAJE = "Pathological Data Input updated.";
                    resultado.STATUS = "success";
                    return resultado;

                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {

                    var sqlex = ex.InnerException.InnerException as System.Data.SqlClient.SqlException;
                    var objEncuestasDetalle = (from a in dbContextPathological.EncuestasDetalle where a.EncuestaId == _objEncuestas.EncuestaId select a)
                                                .FirstOrDefault();

                    foreach (var childModel in objEncuestas.EncuestasDetalle)
                    {
                        var existingChild = objEncuestas.EncuestasDetalle
                            .Where(c => c.EncuestaDetalleId == childModel.EncuestaDetalleId && c.EncuestaDetalleId != default(int))
                            .FirstOrDefault();


                        


                    }


                    

                    if (sqlex != null)
                    {
                        switch (sqlex.Number)
                        {
                            case 547:
                                resultado.MENSAJE = "Pathological Data Input cant be updated";
                                break; //FK exception
                            case 2601:
                                resultado.MENSAJE = "Pathological Data Input has same values.";//primary key exception
                                break;
                            case 2627:
                            default: resultado.MENSAJE = ex.Message; break;//otra excepcion que no controlo.
                        }
                    }
                    resultado.STATUS = "error";
                    return resultado;
                }
                catch (System.IO.IOException ex)
                {


                    resultado.STATUS = ex.Message;
                    return resultado;
                }
            }


        }
        */
        /*
        [HttpPost]
        [Route("UpdateHeaderDetail")]
        public Models.Results PostUpdateHeaderDetail([FromBody]Models.Encuestas _objEncuestas)
        {
            using (var dbContextPathological = new Models.ModelHealthAdvisor())
            {
                Models.Results resultado = new Models.Results();
                try
                {
                    objEncuestas = (from a in dbContextPathological.Encuestas.Include("EncuestasDetalle") where a.EncuestaId == _objEncuestas.EncuestaId select a)
                                                .FirstOrDefault();


                    var objEncuestasDetalle = (from a in dbContextPathological.EncuestasDetalle where a.EncuestaId == _objEncuestas.EncuestaId select a)
                                                .FirstOrDefault();


                    if (objEncuestas != null)
                    {

                        var PathologicaDetailList = objEncuestas.EncuestasDetalle
                                .Select(z => z.EncuestaDetalleId).ToList();

                        //update Items

                        
                        dbContextPathological.EncuestasDetalle.RemoveRange(objEncuestas.EncuestasDetalle);

                        //Encuestas Details
                        foreach (var _objEncuestasDetalle in _objEncuestas.EncuestasDetalle)
                        {
                            



                            

                            

                            //find header
                            _objEncuestasDetalle.EncuestaId = _objEncuestas.EncuestaId;
                            dbContextPathological.EncuestasDetalle.Add(_objEncuestasDetalle);
                            
                        }

                        //update header
                        objEncuestas.Observacion = _objEncuestas.Observacion;
                        objEncuestas.LocalId = _objEncuestas.LocalId;
                        objEncuestas.EncargadoId = _objEncuestas.EncargadoId;                        
                        objEncuestas.ModificadoAl = DateTime.Now;
                        objEncuestas.UserModifierId = TokenGenerator.GetUserSystem(Request);
                        dbContextPathological.Encuestas.Attach(objEncuestas);
                        dbContextPathological.Entry(objEncuestas).Property(x => x.Observacion).IsModified = true;
                        dbContextPathological.Entry(objEncuestas).Property(x => x.LocalId).IsModified = true;
                        dbContextPathological.Entry(objEncuestas).Property(x => x.EncargadoId).IsModified = true;                        
                        dbContextPathological.Entry(objEncuestas).Property(x => x.ModificadoAl).IsModified = true;
                        dbContextPathological.Entry(objEncuestas).Property(x => x.UserModifierId).IsModified = true;

                        //update detail

                        dbContextPathological.SaveChanges();
                    }
                    resultado.OBJETO = objEncuestas;
                    resultado.MENSAJE = "Pathological Data Input updated.";
                    resultado.STATUS = "success";
                    return resultado;

                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {

                    var sqlex = ex.InnerException.InnerException as System.Data.SqlClient.SqlException;
                    var objEncuestasDetalle = (from a in dbContextPathological.EncuestasDetalle where a.EncuestaId == _objEncuestas.EncuestaId select a)
                                                .FirstOrDefault();

                    foreach (var childModel in objEncuestas.EncuestasDetalle)
                    {
                        var existingChild = objEncuestas.EncuestasDetalle
                            .Where(c => c.EncuestaDetalleId == childModel.EncuestaDetalleId && c.EncuestaDetalleId != default(int))
                            .FirstOrDefault();


                        if (existingChild != null)
                        {

                            //childModel.pdidIsDeleted = false;
                            //dbContextPathological.Entry(existingChild).CurrentValues.SetValues(childModel);

                        }


                    }


                    

                    if (sqlex != null)
                    {
                        switch (sqlex.Number)
                        {
                            case 547:
                                resultado.MENSAJE = "Pathological Data Input cant be updated";
                                break; //FK exception
                            case 2601:
                                resultado.MENSAJE = "Pathological Data Input has same values.";//primary key exception
                                break;
                            case 2627:
                            default: resultado.MENSAJE = ex.Message; break;//otra excepcion que no controlo.
                        }
                    }
                    resultado.STATUS = "error";
                    return resultado;
                }
                catch (System.IO.IOException ex)
                {
                    resultado.STATUS = ex.Message;
                    return resultado;
                }
            }
        }
        */

        [HttpPost]
        [Route("Delete")]
        public Models.Results PostDelete([FromBody]Models.Encuestas _objEncuestas)
        {
            using (var dbContextPathological = new Models.ModelHealthAdvisor())
            {
                Models.Results resultado = new Models.Results();
                try
                {
                    return null;
                    ////fetching and filter specific member id record   
                    //objEncuestas = (from a in dbContext.Encuestas where a.anId == _objEncuestas.anId select a).FirstOrDefault();
                    ////checking fetched or not with the help of NULL or NOT.  
                    //if (objEncuestas != null)
                    //{
                    //    dbContext.Encuestas.Remove(objEncuestas);
                    //    dbContext.SaveChanges();
                    //    resultado.OBJETO = objEncuestas;
                    //    resultado.MENSAJE = "Tipo de Análisis eliminada exitosamente";
                    //    resultado.STATUS = "success";
                    //    return resultado;
                    //}
                    //else
                    //{

                    //    resultado.MENSAJE = "No existe Tipo de Análisis con el Identificador indicado";
                    //    resultado.STATUS = "error";
                    //    return resultado;
                    //}             
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {

                    var sqlex = ex.InnerException.InnerException as System.Data.SqlClient.SqlException;

                    if (sqlex != null)
                    {
                        switch (sqlex.Number)
                        {
                            case 547:
                                resultado.MENSAJE = "Tipo de Análisis no puede ser eliminada.";
                                break; //FK exception
                            case 2601:
                                resultado.MENSAJE = "Ya existe un Tipo de análisis con el mismo valor de código o nombre.";//primary key exception
                                break;
                            case 2627:
                            default: resultado.MENSAJE = ex.Message; break;//otra excepcion que no controlo.
                        }
                    }
                    resultado.STATUS = "error";
                    return resultado;
                }
            }
        }

        [HttpPost]
        [Route("DeleteLogical")]
        public Models.Results PostDeleteLogical([FromBody]Models.Encuestas _objEncuestas)
        {
            using (var dbContextPathological = new Models.ModelHealthAdvisor())
            {
                Models.Results resultado = new Models.Results();
                try
                {

                    //fetching and filter specific member id record   
                    objEncuestas = (from a in dbContextPathological.Encuestas where a.EncuestaId == _objEncuestas.EncuestaId select a).FirstOrDefault();
                    //checking fetched or not with the help of NULL or NOT.  
                    if (objEncuestas != null)
                    {
                        objEncuestas.UserDeleterId = TokenGenerator.GetUserSystem(Request);
                        objEncuestas.EliminadoAl = System.DateTime.Now;
                        objEncuestas.EstaEliminado = true;
                        dbContextPathological.SaveChanges();
                        resultado.OBJETO = objEncuestas;
                        resultado.MENSAJE = "Pathological Data Input  disabled";
                        resultado.STATUS = "success";
                        return resultado;
                    }
                    else
                    {

                        resultado.MENSAJE = "Pathological Data Input no exist";
                        resultado.STATUS = "error";
                        return resultado;
                    }
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {

                    var sqlex = ex.InnerException.InnerException as System.Data.SqlClient.SqlException;

                    if (sqlex != null)
                    {
                        switch (sqlex.Number)
                        {
                            case 547:
                                resultado.MENSAJE = "Pathological Data Input cannot be disabled.";
                                break; //FK exception
                            case 2601:
                                resultado.MENSAJE = "Ya existe un Tipo de análisis con el mismo valor de código o nombre.";//primary key exception
                                break;
                            case 2627:
                            default: resultado.MENSAJE = ex.Message; break;//otra excepcion que no controlo.
                        }
                    }

                    resultado.STATUS = "error";
                    return resultado;
                }
            }
        }
    }
}
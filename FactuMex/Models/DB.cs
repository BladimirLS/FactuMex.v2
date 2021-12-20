using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CallCenter.Models;
using Microsoft.Extensions.Configuration;
namespace CallCenter.Models
{
    public enum Cns
    {
        Inb,
        Renomax,
        MailMax,
        MetodosPago
    }
    public class DB
    {
        private string Q { get; set; }
        private string Cn { get; set; }
        private SqlCommand cmd { get; set; }
        private SqlConnection con { get; set; }
        public DB(Cns DB, string Query, object[] Params)
        {
            SelectCn(DB);
            switch (Query)
            {
                case "CreaNCF":
                    string IDEmpresa = (Params[0]).ToString();
                    Q = Queries.CreaNCF(IDEmpresa);
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "ActualizaNCF":
                    string IDEmpresa2 = (Params[0]).ToString();
                    string sec = (Params[1]).ToString();
                    string NCF = (Params[2]).ToString();
                    string IDFactura = (Params[3]).ToString();
                    Q = Queries.ActualizaNCF(IDEmpresa2,sec,NCF,IDFactura);
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "ValidaToken":
                    string UsuarioVT = (Params[0]).ToString();
                    string TokenVT = (Params[1]).ToString();
                    SqlParameter UsuarioVTP = new SqlParameter("@USUARIO", SqlDbType.NVarChar, 50);
                    SqlParameter TokenVTP = new SqlParameter("@TOKEN", SqlDbType.NVarChar, 50);
                    UsuarioVTP.Value = UsuarioVT;
                    TokenVTP.Value = TokenVT;
                    Q = Queries.ValidaToken();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(UsuarioVTP);
                    cmd.Parameters.Add(TokenVTP);
                    break;
                case "DescargaAnexo":
                    string indexDA = (Params[0]).ToString();
                    SqlParameter indexDAP = new SqlParameter("@ID", SqlDbType.NVarChar, 50);
                    indexDAP.Value = indexDA;
                    Q = Queries.DescargaAnexo();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(indexDAP);
                    break;
                case "BorraAnexo":
                    string indexBA = (Convert.ToInt32(Params[0]) + 1).ToString();
                    SqlParameter indexBAP = new SqlParameter("@ID", SqlDbType.NVarChar, 50);
                    indexBAP.Value = indexBA;
                    Q = Queries.BorraAnexo();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(indexBAP);
                    break;
                case "CambiaMetodoPago":
                    string PuidCMP = Params[0].ToString();
                    string MetodoCMP = Params[1].ToString();
                    SqlParameter MetodoCMPP = new SqlParameter("@METODO", SqlDbType.NVarChar, 50);
                    MetodoCMPP.Value = MetodoCMP;
                    SqlParameter PuidCMPP = new SqlParameter("@PUID", SqlDbType.NVarChar, 50);
                    PuidCMPP.Value = PuidCMP;
                    Q = Queries.CambiaMetodoPago();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(MetodoCMPP);
                    cmd.Parameters.Add(PuidCMPP);
                    break;
                case "BuscaMetodoPago":
                    string BuscBPM = Params[0].ToString();
                    SqlParameter BuscarBMP = new SqlParameter("@BUSCAR", SqlDbType.NVarChar, 50);
                    BuscarBMP.Value = BuscBPM;
                    if (BuscBPM != "BRINGITALL")
                        Q = Queries.BuscaMetodoPago();
                    else
                        Q = Queries.BuscaMetodoPagoTodo();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(BuscarBMP);
                    break;
                case "CargaFisicaAct":
                    Q = Queries.GetFisicaAct();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaFisicaAnt":
                    Q = Queries.GetFisicaAnt();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaEnvAct":
                    Q = Queries.GetEnvAct();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaEnvAnt":
                    Q = Queries.GetEnvAnt();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaColAct":
                    Q = Queries.GetColAct();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaColAnt":
                    Q = Queries.GetColAnt();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaCliDB":
                    Q = Queries.GetCliDB();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaCliAct":
                    Q = Queries.GetCliAct();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaCliAnt":
                    Q = Queries.GetCliAnt();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaAuxsAct":
                    Q = Queries.ExcAct();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaAuxsAnt":
                    Q = Queries.ExtAnt();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaSolicitudes":
                    Q = Queries.SolAct();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaCartPend":
                    Q = Queries.CartPend();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaCartPendEnvio":
                    Q = Queries.CartPendEnvio();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaCartNotReady":
                    Q = Queries.CartNotReady();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaEnviosPendxNombre":
                    string Nom = Params[0].ToString();
                    SqlParameter NomCEPN = new SqlParameter("@NOMBRE", SqlDbType.NVarChar, 50);
                    NomCEPN.Value = Nom;
                    Q = Queries.CargaEnviosPendxNombre();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(NomCEPN);
                    break;
                case "CargaAno":
                    Q = Queries.CargaAno();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaStatEnvios":
                    Q = Queries.StatEnviados();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaStatReno":
                    Q = Queries.StatReno();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaEnviosPend":
                    Q = Queries.CargaEnviosPend();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "GuardaConfig":
                    int Conf = (int)Params[0];
                    SqlParameter AnoGC = new SqlParameter("@ANO", SqlDbType.BigInt);
                    AnoGC.Value = Conf;
                    Q = Queries.GuardaConfigAno();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(AnoGC);
                    break;
                case "GuardaAnexo":
                    Anexo an = (Anexo)Params[0];
                    SqlParameter AnexoGA = new SqlParameter("@NOMBRE", SqlDbType.NVarChar, 250);
                    AnexoGA.Value = an.Arc.Nombre;
                    byte[] contenido = Convert.FromBase64String(an.Arc.Base64Url);
                    SqlParameter ContenidoGA = new SqlParameter("@CONTENIDO", SqlDbType.VarBinary, contenido.Length);
                    ContenidoGA.Value = contenido;
                    SqlParameter IndiceGA = new SqlParameter("@INDICE", SqlDbType.BigInt);
                    IndiceGA.Value = an.Index + 1;
                    Q = Queries.GuardaAnexo();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(AnexoGA);
                    cmd.Parameters.Add(ContenidoGA);
                    cmd.Parameters.Add(IndiceGA);
                    break;
                case "CargaAnexos":
                    Q = Queries.CargaAnexos();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "GuardaAsignacion":
                    DTOAsignacion Asig = (DTOAsignacion)Params[0];
                    SqlParameter IDMovItemAsig = new SqlParameter("@IDMOVITEM", SqlDbType.BigInt);
                    IDMovItemAsig.Value = Asig.idmovitem;
                    SqlParameter AsignacionGA = new SqlParameter("@ASIGNACION", SqlDbType.Float);
                    AsignacionGA.Value = Asig.asignacion;
                    Q = Queries.GuardaAsignacion();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(IDMovItemAsig);
                    cmd.Parameters.Add(AsignacionGA);
                    break;
                case "CargaUltOperacionRN":
                    string CodPolizaCUO = Params[0].ToString();
                    SqlParameter PolizaCUO = new SqlParameter("@CODPOLIZA", SqlDbType.NVarChar, 50);
                    PolizaCUO.Value = CodPolizaCUO;
                    Q = Queries.CargaUltOperacionRN();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(PolizaCUO);
                    break;
                case "ValidaEmail":
                    string CodPolizaVE = Params[0].ToString();
                    SqlParameter PolizaVE = new SqlParameter("@CODPOLIZA", SqlDbType.NVarChar, 50);
                    PolizaVE.Value = CodPolizaVE;
                    Q = Queries.ValidaEmail();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(PolizaVE);
                    break;
                case "GuardaSeleccionPlantilla":
                    DTOSelPlantilla Sel = (DTOSelPlantilla)Params[0];
                    SqlParameter Poliza5 = new SqlParameter("@CODPOLIZA", SqlDbType.NVarChar, 50);
                    Poliza5.Value = Sel.poliza;
                    SqlParameter Plantilla3 = new SqlParameter("@PLANTILLA", SqlDbType.NVarChar, 50);
                    Plantilla3.Value = Sel.plantilla;
                    SqlParameter Fisica = new SqlParameter("@FISICO", SqlDbType.Bit);
                    Fisica.Value = Sel.fisica;
                    Q = Queries.GuardaPlantillaSel();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(Poliza5);
                    cmd.Parameters.Add(Plantilla3);
                    cmd.Parameters.Add(Fisica);
                    break;
                case "BuscaCliente":
                    Q = Queries.BuscaCliente(Params[0].ToString());
                    break;
                case "CargaArchivos":
                    Q = Queries.CargaArchivo();
                    break;
                case "CargaOperacion":
                    Q = Queries.CargaOperacion(Params[0].ToString());
                    break;
                case "CargaVehiculo":
                    Q = Queries.CargaVehiculos(Params[0].ToString());
                    break;
                case "ValidaRenoExiste":
                    Q = Queries.ValidaRenoExiste(Params[0].ToString());
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "GetAnoActual":
                    Q = Queries.GetAnoActual();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "GetRenoAnoActual":
                    string CodPoliza6 = Params[0].ToString();
                    string AnoActual = Params[1].ToString();
                    SqlParameter Poliza7 = new SqlParameter("@CODPOLIZA", SqlDbType.NVarChar, 50);
                    SqlParameter AnoActual2 = new SqlParameter("@ANO", SqlDbType.Int);
                    Poliza7.Value = CodPoliza6;
                    AnoActual2.Value = int.Parse(AnoActual);
                    Q = Queries.GetExisteAnoActual();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(Poliza7);
                    cmd.Parameters.Add(AnoActual2);
                    break;
                case "GetPlantilla":
                    string CodPoliza5 = Params[0].ToString();
                    SqlParameter Poliza6 = new SqlParameter("@CODPOLIZA", SqlDbType.NVarChar, 50);
                    Poliza6.Value = CodPoliza5;
                    Q = Queries.GetPlantillaxCodPoliza();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(Poliza6);
                    break;
                case "GetAU":
                    string CodPoliza8 = Params[0].ToString();
                    SqlParameter Poliza8 = new SqlParameter("@CODPOLIZA", SqlDbType.NVarChar, 50);
                    Poliza8.Value = CodPoliza8;
                    Q = Queries.GetAU();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(Poliza8);
                    break;
                case "ValidaRenoExisteParametrizada":
                    string CodPoliza4 = Params[0].ToString();
                    string Ano = Params[1].ToString();
                    string Pol = "";
                    string AU = "";
                    if (CodPoliza4.Contains("AU-"))
                        Pol = GetCodPoliza(CodPoliza4, "AU-");
                    else if (CodPoliza4.Contains("AUXS-"))
                    {
                        Pol = GetCodPoliza(CodPoliza4, "AUXS-");
                        AU = new DB(Cns.Inb, "GetAU", new List<object>() { Pol }.ToArray()).EjecutaCmd().ToString();
                    }
                    else
                        throw new Exception("Código de Poliza Invalido");

                    string PlanPol = Pol;
                    if (PlanPol.Contains("AUXS"))
                        PlanPol = AU;
                    string Plantilla4 = new DB(Cns.Renomax, "GetPlantilla", new List<object>() { PlanPol }.ToArray()).EjecutaCmd().ToString();
                    SqlParameter Poliza4 = new SqlParameter("@CODPOLIZA", SqlDbType.NVarChar, 50);
                    Poliza4.Value = Pol;
                    string Existe = new DB(Cns.Renomax, "GetRenoAnoActual", new List<object>() { Pol, Ano }.ToArray()).EjecutaCmd().ToString();
                    Q = Queries.ValidaRenoExisteParametrizada(Ano, Plantilla4, Existe, AU);
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(Poliza4);
                    break;
                case "CargaPlantillas":
                    Q = Queries.CargaPlantillas();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "GuardaAU":
                    string CodPoliza = Params[0].ToString();
                    if (CodPoliza.Contains("AUXS-"))
                        Q = Queries.GuardaAUXS();
                    if (CodPoliza.Contains("AU-"))
                        Q = Queries.GuardaAU();
                    SqlParameter Poliza = new SqlParameter("@CODPOLIZA", SqlDbType.NVarChar, 50);
                    Poliza.Value = CodPoliza;
                    SqlParameter FName = new SqlParameter("@FILENAME", SqlDbType.NVarChar, 250);
                    FName.Value = Params[1].ToString();
                    byte[] bytes = (byte[])Params[2];
                    SqlParameter FBytes = new SqlParameter("@FILEBYTES", SqlDbType.VarBinary, bytes.Length);
                    FBytes.Value = bytes;
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(Poliza);
                    cmd.Parameters.Add(FName);
                    cmd.Parameters.Add(FBytes);
                    break;
                case "GuardaArchivo":
                    string Ruta = Params[0].ToString();
                    string Nombre = Params[1].ToString();
                    string Fecha = Params[2].ToString();
                    Q = Queries.GuardaArchivo();
                    SqlParameter PRuta = new SqlParameter("@RUTA", SqlDbType.NVarChar, 1000);
                    PRuta.Value = Ruta;
                    SqlParameter PNombre = new SqlParameter("@NOMBRE", SqlDbType.NVarChar, 500);
                    PNombre.Value = Nombre;
                    SqlParameter PFecha = new SqlParameter("@FECHA", SqlDbType.DateTime);
                    PFecha.Value = Fecha;
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(PRuta);
                    cmd.Parameters.Add(PNombre);
                    cmd.Parameters.Add(PFecha);
                    break;
                case "EnviaMail":
                    Q = Queries.EnviaMail();
                    string CodPoliza2 = Params[0].ToString();
                    SqlParameter Poliza2 = new SqlParameter("@CODPOLIZA", SqlDbType.NVarChar, 50);
                    Poliza2.Value = CodPoliza2;
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(Poliza2);
                    break;
                case "CargaCorreo":
                    Q = Queries.CargaCorreo();
                    string CodPoliza3 = Params[0].ToString();
                    SqlParameter Poliza3 = new SqlParameter("@CODPOLIZA", SqlDbType.NVarChar, 50);
                    Poliza3.Value = CodPoliza3;
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(Poliza3);
                    break;
                case "GuardaRelOrigenDestino":
                    Q = Queries.GuardaRelOrigenDestino();
                    string Origen = Params[0].ToString();
                    string Destino = Params[1].ToString();
                    string Plantilla = Params[2].ToString();
                    SqlParameter OrigenP = new SqlParameter("@ORIGEN", SqlDbType.NVarChar, 500);
                    OrigenP.Value = Origen;
                    SqlParameter DestinoP = new SqlParameter("@DESTINO", SqlDbType.NVarChar, 500);
                    DestinoP.Value = Destino;
                    SqlParameter PlantillaP = new SqlParameter("@PLANTILLA", SqlDbType.NVarChar, 500);
                    PlantillaP.Value = Plantilla;
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(OrigenP);
                    cmd.Parameters.Add(DestinoP);
                    cmd.Parameters.Add(PlantillaP);
                    break;
                case "GuardaUsuario":
                    Usuario U = (Usuario)Params[0];
                    if (U.IDUsuario == "0")
                        Q = Queries.GuardaUsuario();
                    else
                        Q = Queries.ActualizaUsuario();
                    SqlParameter IDUsuarioP = new SqlParameter("@IDUSUARIO", SqlDbType.NVarChar, 500);
                    IDUsuarioP.Value = U.IDUsuario;
                    SqlParameter NombreP = new SqlParameter("@NOMBRE", SqlDbType.NVarChar, 500);
                    NombreP.Value = U.Nombre;
                    SqlParameter EmailP = new SqlParameter("@EMAIL", SqlDbType.NVarChar, 500);
                    EmailP.Value = U.Email;
                    SqlParameter PwdP = new SqlParameter("@PWD", SqlDbType.NVarChar, 500);
                    PwdP.Value = U.Pwd;
                    SqlParameter CedulaP = new SqlParameter("@CEDULA", SqlDbType.NVarChar, 500);
                    CedulaP.Value = U.Cedula;
                    SqlParameter TelefonoP = new SqlParameter("@TELEFONO", SqlDbType.NVarChar, 500);
                    TelefonoP.Value = U.Telefono;
                    SqlParameter FotoP = new SqlParameter("@FOTO", SqlDbType.NVarChar, 500);
                    FotoP.Value = U.Foto;

                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(IDUsuarioP);
                    cmd.Parameters.Add(NombreP);
                    cmd.Parameters.Add(EmailP);
                    cmd.Parameters.Add(PwdP);
                    cmd.Parameters.Add(CedulaP);
                    cmd.Parameters.Add(TelefonoP);
                    cmd.Parameters.Add(FotoP);
                    break;
                case "GuardaRels":
                    Rels Relaciones = (Rels)Params[0];
                    con = new SqlConnection(Cn);
                    con.Open();
                    var tr = con.BeginTransaction();
                    Relaciones.Relaciones.ToList().ForEach(r =>
                    {
                        new DB(Cns.Renomax, "GuardaRelOrigenDestino", new List<object>() { r.Origen, r.Destino, r.Plantilla }.ToArray())
                        .EjecutaCmdNoCn(con, tr);
                    });
                    tr.Commit();
                    con.Close();
                    break;
                case "CargaRel":
                    Q = Queries.CargaRelOrigenDestino();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "BorraRel":
                    Q = Queries.BorraRelOrigenDestino();
                    string Plantilla2 = Params[0].ToString();
                    SqlParameter PlantillaP2 = new SqlParameter("@PLANTILLA", SqlDbType.NVarChar, 500);
                    PlantillaP2.Value = Plantilla2;
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(PlantillaP2);
                    break;
                case "CargaUsuarios":
                    string Buscar;
                    if (Params != null)
                    {
                        Buscar = Params[0].ToString();
                        SqlParameter BuscarP = new SqlParameter("@BUSCAR", SqlDbType.NVarChar, 500);
                        BuscarP.Value = Buscar;
                        Q = Queries.CargaUsuariosxBusqueda();
                        con = new SqlConnection(Cn);
                        cmd = new SqlCommand(Q, con);
                        cmd.Parameters.Add(BuscarP);
                    }
                    else
                    {
                        Q = Queries.CargaUsuarios();
                        con = new SqlConnection(Cn);
                        cmd = new SqlCommand(Q, con);
                    }

                    break;
                case "CargaUsuariosxEmail":
                    string Buscar2;
                    if (Params != null)
                    {
                        Buscar2 = Params[0].ToString();
                        SqlParameter BuscarP2 = new SqlParameter("@BUSCAR", SqlDbType.NVarChar, 500);
                        BuscarP2.Value = Buscar2;
                        Q = Queries.CargaUsuariosxEmail();
                        con = new SqlConnection(Cn);
                        cmd = new SqlCommand(Q, con);
                        cmd.Parameters.Add(BuscarP2);
                    }
                    break;
                case "ValidaUsuario":
                    DTOIniciaSesion Sesion = (DTOIniciaSesion)Params[0];
                    SqlParameter EmailP2 = new SqlParameter("@EMAIL", SqlDbType.NVarChar, 500);
                    EmailP2.Value = Sesion.Email;
                    SqlParameter PwdP2 = new SqlParameter("@PWD", SqlDbType.NVarChar, 500);
                    PwdP2.Value = Sesion.Pwd;
                    Q = Queries.VerificaUsuario();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(EmailP2);
                    cmd.Parameters.Add(PwdP2);
                    break;
                case "ValidaSesion":
                    DTOSesion SesionVS = (DTOSesion)Params[0];
                    SqlParameter TokenVS = new SqlParameter("@TOKEN", SqlDbType.NVarChar, 500);
                    TokenVS.Value = SesionVS.Token;
                    SqlParameter MetaVS = new SqlParameter("@META", SqlDbType.NVarChar, 2048);
                    MetaVS.Value = SesionVS.Meta;
                    Q = Queries.ValidaUsuario();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(TokenVS);
                    cmd.Parameters.Add(MetaVS);
                    break;
                case "IniciaSesion":
                    string Meta = Params[1].ToString();
                    SqlParameter MetaVU = new SqlParameter("@META", SqlDbType.NVarChar, 2048);
                    MetaVU.Value = Meta;
                    string Email3 = Params[0].ToString();
                    SqlParameter EmailP3 = new SqlParameter("@EMAIL", SqlDbType.NVarChar, 500);
                    EmailP3.Value = Email3;
                    SqlParameter CodSesionP = new SqlParameter("@CODSESION", SqlDbType.NVarChar, 500);
                    CodSesionP.Value = Guid.NewGuid().ToString().Replace("-", "");
                    SqlParameter ExpiraP = new SqlParameter("@EXPIRA", SqlDbType.NVarChar, 500);
                    ExpiraP.Value = DateTime.Now.AddHours(12);
                    Q = Queries.IniciaSesion();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(EmailP3);
                    cmd.Parameters.Add(CodSesionP);
                    cmd.Parameters.Add(ExpiraP);
                    cmd.Parameters.Add(MetaVU);
                    break;
                case "RetornaSesion":
                    long IDSesion = Convert.ToInt32(Params[0]);
                    SqlParameter IDSesionP = new SqlParameter("@IDSESION", SqlDbType.NVarChar, 500);
                    IDSesionP.Value = IDSesion;
                    Q = Queries.RetornaSesion();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(IDSesionP);
                    break;
                case "CargaPermisosUsuario":
                    long IDUsuario = Convert.ToInt32(Params[0]);
                    SqlParameter IDUsuarioP2 = new SqlParameter("@IDUSUARIO", SqlDbType.NVarChar, 500);
                    IDUsuarioP2.Value = IDUsuario;
                    Q = Queries.CargaPermisosUsuario();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(IDUsuarioP2);
                    break;
                case "CargaPermisosPlantilla":
                    Q = Queries.CargaPermisosPlantilla();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "GuardaPermisosUsuario":
                    DTOTogglePermiso Permiso = (DTOTogglePermiso)(Params[0]);
                    SqlParameter IDUsuarioP3 = new SqlParameter("@IDUSUARIO", SqlDbType.NVarChar, 500);
                    IDUsuarioP3.Value = Permiso.IDUsuario;
                    SqlParameter PermisoP = new SqlParameter("@PERMISO", SqlDbType.NVarChar, 500);
                    PermisoP.Value = Permiso.Permiso;
                    SqlParameter EstatusP = new SqlParameter("@ESTATUS", SqlDbType.NVarChar, 500);
                    EstatusP.Value = Permiso.Estatus;
                    Q = Queries.GuardaPermisosUsuario();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(IDUsuarioP3);
                    cmd.Parameters.Add(PermisoP);
                    cmd.Parameters.Add(EstatusP);
                    break;
                case "CargaPermisosxEmail":
                    string Email4 = Params[0].ToString();
                    SqlParameter EmailP4 = new SqlParameter("@EMAIL", SqlDbType.NVarChar, 500);
                    EmailP4.Value = Email4;
                    Q = Queries.CargaPermisosxEmail();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(EmailP4);
                    break;
                case "CargaAsignaciones":
                    string Cedula = Params[0].ToString();
                    float Asignacion = float.Parse(Params[1].ToString());
                    SqlParameter CedulaP2 = new SqlParameter("@CEDULA", SqlDbType.NVarChar, 500);
                    CedulaP2.Value = Cedula;
                    SqlParameter CedulaSinGui = new SqlParameter("@CEDULASINGUI", SqlDbType.NVarChar, 500);
                    CedulaSinGui.Value = Cedula.Replace("-", "");
                    SqlParameter AsignacionP = new SqlParameter("@VALOR", SqlDbType.Float);
                    AsignacionP.Value = Asignacion;
                    Q = Queries.CargaAsignacion();
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    cmd.Parameters.Add(CedulaP2);
                    cmd.Parameters.Add(CedulaSinGui);
                    cmd.Parameters.Add(AsignacionP);
                    break;
                case "ActualizaEnte":
                    string Rels = Params[0].ToString();
                    string Tabla = Params[1].ToString();
                    string Cond = Params[2].ToString();
                    Q = Queries.ActualizaEnte(Rels, Tabla, Cond);
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
                case "CargaAsignacionesTODAS":
                    DTOCampos Campos = (DTOCampos)Params[0];
                    string strCampos = "";
                    var ult = Campos.Campos.Last();
                    Campos.Campos.ToList().ForEach(c =>
                    {
                        if (ult == c)
                        {
                            strCampos = strCampos + $"{c.Origen} '{c.Destino}'";

                        }
                        else
                        {
                            strCampos = strCampos + $"{c.Origen} '{c.Destino}',";
                        }
                    });
                    string strConsulta = $"SELECT {strCampos} FROM [dbo].[{Campos.TablaOrigen}]";
                    con = new SqlConnection(Cn);
                    Q = strConsulta;
                    cmd = new SqlCommand(Q, con);
                    break;
                case "ActualizaDatosTODAS":
                    DTOCampos Campos2 = (DTOCampos)Params[0];
                    string strCampos2 = "";
                    var ult2 = Campos2.Campos.Last();
                    string NroFiscal = "";
                    Campos2.Campos.ToList().ForEach(c =>
                    {
                        if (c.Destino == "NroFiscal")
                            NroFiscal = c.Origen;
                        if (ult2 == c)
                        {
                            strCampos2 = strCampos2 + $"[Inbroker].[dbo].[Tg_Ente].[{c.Destino}]=[MaxMiniCRM].[dbo].[{Campos2.TablaOrigen}].[{c.Origen}]";

                        }
                        else
                        {
                            strCampos2 = strCampos2 + $"[Inbroker].[dbo].[Tg_Ente].[{c.Destino}]=[MaxMiniCRM].[dbo].[{Campos2.TablaOrigen}].[{c.Origen}],";
                        }
                    });
                    Q = Queries.ActualizaEnte(strCampos2, Campos2.TablaOrigen, NroFiscal);
                    con = new SqlConnection(Cn);
                    cmd = new SqlCommand(Q, con);
                    break;
            }
        }

        private string GetCodPoliza(string FileName, string tPoliza)
        {
            List<char> nums = new List<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int iCod = FileName.IndexOf(tPoliza);
            string strRest = FileName.Substring(iCod + tPoliza.Length);
            string strOut = FileName.Substring(iCod, strRest.IndexOf(strRest.Where(c => !nums.Contains(c)).First()) + tPoliza.Length);
            return strOut;
        }

        private DB SelectCn(Cns cn)
        {
            var conf = new ConfigurationBuilder().AddJsonFile(".\\Config.json").Build();
            switch (cn)
            {
                case Cns.Inb:
                    Cn = conf.GetValue<string>("Inbroker");
                    break;
                case Cns.Renomax:
                    Cn = conf.GetValue<string>("Renomax");
                    break;
                case Cns.MailMax:
                    Cn = conf.GetValue<string>("Mailmax");
                    break;
                case Cns.MetodosPago:
                    Cn = conf.GetValue<string>("MetodosPago");
                    break;
            }
            return this;
        }
        public DataTable RetornaDTxP()
        {
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Result);
            return Result;
        }
        public DataTable RetornaDT()
        {
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(Q, Cn);
            da.Fill(Result);
            return Result;
        }
        public object EjecutaCmd()
        {
            con.Open();
            object result = cmd.ExecuteScalar();
            con.Close();
            return result;
        }
        public object EjecutaTSQL()
        {
            con.Open();
            object result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }
        public object EjecutaCmdNoCn(SqlConnection Con, SqlTransaction Tr)
        {
            cmd.Connection = Con;
            cmd.Transaction = Tr;
            object result = cmd.ExecuteScalar();
            return result;
        }
        bool EmailValido(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}

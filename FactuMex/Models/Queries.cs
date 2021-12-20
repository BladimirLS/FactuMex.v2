using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    public static class Queries
    {
        public static string ActualizaNCF(string IDEmpresa,string Sec,string NCF,string IDFactura)
        {
            return $@"  
                        UPDATE  [dbo].[NCF{IDEmpresa}]
                        SET     Secuencial='{NCF}',
                                IDFactura='{IDFactura}'
                        WHERE   ID={Sec};
                        ";
        }
        public static string CreaNCF(string IDEmpresa)
        {
            return $@"  
                        if not exists (select * from sysobjects where name='NCF{IDEmpresa}' and xtype='U')
						BEGIN
                        CREATE TABLE [dbo].[NCF{IDEmpresa}](
	                        [ID] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	                        [Secuencial] [nvarchar](50) NULL,
	                        [IDFactura] [bigint] NULL,
                         CONSTRAINT [PK_NCF{IDEmpresa}] PRIMARY KEY CLUSTERED 
                        (
	                        [ID] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                        ) ON [PRIMARY];
							DECLARE	@START INTEGER;
							SET		@START=(SELECT NUMDESDE FROM DBO.Comprobante WHERE IDComp={IDEmpresa});
							DBCC CHECKIDENT ('NCF{IDEmpresa}', RESEED, @START);  
						END
                        
                        INSERT  INTO [dbo].[NCF{IDEmpresa}]
                        SELECT  'B0000000',
                                0;
                        SELECT  @@IDENTITY;
                        ";
        }
        public static string DescargaAnexo()
        {
            return $@"
                    SELECT * FROM	[FactuMex].dbo.Anexos
                    WHERE	Nombre=@ID;
            ";
        }
        public static string CambiaMetodoPago()
        {
            return $@"
                    UPDATE	[MetodosPago].dbo.MetodoSeleccionado
                    SET		Metodo=@METODO
                    WHERE	IDCliente=@PUID;
            ";
        }
        public static string BuscaMetodoPago()
        { 
        return $@"SELECT  Ano,
		                    R.IdCliente,
		                    Cliente,
		                    Poliza,
		                    PolizaExc,
		                    Correo,
		                    Nomina12,
		                    Montos,
		                    PagoUnico,
		                    Cuotas4,
		                    Descuento,
		                    Metodo,
		                    Fecha,
                            M.IDCliente Puid
                    FROM	[MetodosPago].[dbo].[MetodoSeleccionado] M

                    JOIN	[FactuMex].[dbo].[RenovacionesBPD] R
                    ON		R.UID=M.IDCliente

                    WHERE	CLIENTE LIKE '%'+@BUSCAR+'%'
                    OR		POLIZA=@BUSCAR
                    OR		POLIZAEXC=@BUSCAR
                    ";
        }

        public static string ValidaToken()
        {
            return $@"
                      SELECT ISNULL(1,0) VALIDADO
                      FROM [FactuMex].[dbo].[Sesion] S
  
                      JOIN [FactuMex].[dbo].[RenoUsuario] R
                      ON    R.IDUsuario=S.IDUsuario

                      WHERE S.CodSesion=@TOKEN
                      AND	R.Email=@USUARIO
                      AND	GETDATE()<Expira
                    ";
        }

        public static string BuscaMetodoPagoTodo()
        {
            return $@"SELECT  Ano,
		                    R.IdCliente,
		                    Cliente,
		                    Poliza,
		                    PolizaExc,
		                    Correo,
		                    ROUND(Nomina12,2) Nomina12,
		                    Montos,
		                    ROUND(PagoUnico,2) PagoUnico,
		                    ROUND(Cuotas4,2) Cuotas4,
		                    ROUND(Descuento,2) Descuento,
		                    Metodo,
		                    Fecha,
                            M.IDCliente Puid
                    FROM	[MetodosPago].[dbo].[MetodoSeleccionado] M

                    JOIN	[FactuMex].[dbo].[RenovacionesBPD] R
                    ON		R.UID=M.IDCliente

                    ";
        }

        public static string BorraAnexo()
        {
            return $@"UPDATE [FactuMex].[dbo].[Anexos] SET NOMBRE='AT'+@ID, CONTENIDO=NULL WHERE IDANEXO=@ID;";
        }

        public static string GetEnvAct()
        {
            return $@"SELECT	ORIGEN,DESTINO,ASUNTO,FECHACOLOCADO,FECHAENVIADO 
FROM	[MAILMAX].[DBO].[CORREOS]
WHERE	YEAR(FECHAENVIADO)=YEAR(GETDATE())";
        }
        public static string GetEnvAnt()
        {
            return $@"SELECT	ORIGEN,DESTINO,ASUNTO,FECHACOLOCADO,FECHAENVIADO 
FROM	[MAILMAX].[DBO].[CORREOS]
WHERE	YEAR(FECHAENVIADO)=YEAR(GETDATE())-1";
        }
        public static string GetColAct()
        {
            return $@"SELECT	ORIGEN,DESTINO,ASUNTO,FECHACOLOCADO,FECHAENVIADO 
FROM	[MAILMAX].[DBO].[CORREOS]
WHERE	YEAR(FECHACOLOCADO)=YEAR(GETDATE())";
        }
        public static string GetColAnt()
        {
            return $@"SELECT	ORIGEN,DESTINO,ASUNTO,FECHACOLOCADO,FECHAENVIADO 
FROM	[MAILMAX].[DBO].[CORREOS]
WHERE	YEAR(FECHACOLOCADO)=YEAR(GETDATE())-1";
        }
        public static string StatEnviados()
        {
            return $@"
                        SELECT	(SELECT	COUNT(*)
		                        FROM	[MailMax].DBO.Correos
		                        WHERE	YEAR(FechaEnviado)=YEAR(GETDATE())) ENVACTUAL,
		                        (SELECT	COUNT(*)
		                        FROM	[MailMax].DBO.Correos
		                        WHERE	YEAR(FechaEnviado)=YEAR(GETDATE())-1) ENVANTERIOR,
		                        (SELECT	COUNT(*)
		                        FROM	[MailMax].DBO.Correos
		                        WHERE	YEAR(FechaColocado)=YEAR(GETDATE())) COLACTUAL,
		                        (SELECT	COUNT(*)
		                        FROM	[MailMax].DBO.Correos
		                        WHERE	YEAR(FechaColocado)=YEAR(GETDATE())-1) COLANTERIOR
                    ";
        }
        public static string GetFisicaAct()
        {
            return $@"SELECT [IDCliente]
                      ,[Cliente]
                      ,[Poliza]
                      ,[PolizaExc]
                      ,[Correo]
                      ,[Vehiculos]
                      ,[Asignacion]
                      ,[Enviado]
                      ,[FechaEnvio]
                      ,[Envios]
                      ,[SinAux]
                      ,[Plantilla]
                      ,[Fisica]
                  FROM [FactuMex].[dbo].[RenovacionesBPD]
                  WHERE	YEAR(FechaEnvio)=YEAR(GETDATE())
                  AND FISICA=1";
        }
        public static string GetFisicaAnt()
        {
            return $@"SELECT [IDCliente]
                      ,[Cliente]
                      ,[Poliza]
                      ,[PolizaExc]
                      ,[Correo]
                      ,[Vehiculos]
                      ,[Asignacion]
                      ,[Enviado]
                      ,[FechaEnvio]
                      ,[Envios]
                      ,[SinAux]
                      ,[Plantilla]
                      ,[Fisica]
                  FROM [FactuMex].[dbo].[RenovacionesBPD]
                  WHERE	YEAR(FechaEnvio)=YEAR(GETDATE())-1
                   AND FISICA=1";
        }
        public static string GetCliDB()
        {
            return $@"SELECT [IDCliente]
                      ,[Cliente]
                      ,[Poliza]
                      ,[PolizaExc]
                      ,[Correo]
                      ,[Vehiculos]
                      ,[Asignacion]
                      ,[Enviado]
                      ,[FechaEnvio]
                      ,[Envios]
                      ,[SinAux]
                      ,[Plantilla]
                      ,[Fisica]
                  FROM [FactuMex].[dbo].[RenovacionesBPD]";
        }
        public static string GetCliAct()
        {
            return $@"SELECT [IDCliente]
      ,[Cliente]
      ,[Poliza]
      ,[PolizaExc]
      ,[Correo]
      ,[Vehiculos]
      ,[Asignacion]
      ,[Enviado]
      ,[FechaEnvio]
      ,[Envios]
      ,[SinAux]
      ,[Plantilla]
      ,[Fisica]
  FROM [FactuMex].[dbo].[RenovacionesBPD]
  WHERE	YEAR(FechaEnvio)=YEAR(GETDATE())";
        }
        public static string GetCliAnt()
        {
            return $@"SELECT [IDCliente]
      ,[Cliente]
      ,[Poliza]
      ,[PolizaExc]
      ,[Correo]
      ,[Vehiculos]
      ,[Asignacion]
      ,[Enviado]
      ,[FechaEnvio]
      ,[Envios]
      ,[SinAux]
      ,[Plantilla]
      ,[Fisica]
  FROM [FactuMex].[dbo].[RenovacionesBPD]
  WHERE	YEAR(FechaEnvio)=YEAR(GETDATE())-1";
        }
        public static string ExcAct()
        {
            return $@"
SELECT [IDCliente]
      ,[Cliente]
      ,[Poliza]
      ,[PolizaExc]
      ,[Correo]
      ,[Vehiculos]
      ,[Asignacion]
      ,[Enviado]
      ,[FechaEnvio]
      ,[Envios]
      ,[SinAux]
      ,[Plantilla]
      ,[Fisica]
  FROM [FactuMex].[dbo].[RenovacionesBPD]
  WHERE	YEAR(FechaEnvio)=YEAR(GETDATE())
  AND NOT PolizaExc IS NULL
";
        }
        public static string ExtAnt()
        {
            return $@"
SELECT [IDCliente]
      ,[Cliente]
      ,[Poliza]
      ,[PolizaExc]
      ,[Correo]
      ,[Vehiculos]
      ,[Asignacion]
      ,[Enviado]
      ,[FechaEnvio]
      ,[Envios]
      ,[SinAux]
      ,[Plantilla]
      ,[Fisica]
  FROM [FactuMex].[dbo].[RenovacionesBPD]
  WHERE	YEAR(FechaEnvio)=YEAR(GETDATE())-1
  AND NOT PolizaExc IS NULL
";
        }
        public static string SolAct()
        {
            return $@"
SELECT [IDCliente]
      ,[Cliente]
      ,[Poliza]
      ,[PolizaExc]
      ,[Correo]
      ,[Vehiculos]
      ,[Asignacion]
      ,[Enviado]
      ,[FechaEnvio]
      ,[Envios]
      ,[SinAux]
      ,[Plantilla]
      ,[Fisica]
  FROM [FactuMex].[dbo].[RenovacionesBPD]
  WHERE	IDRenovacion>4059 OR YEAR(FechaEnvio)=YEAR(GETDATE())
";
        }
        public static string CartPend()
        {
            return $@"
SELECT [IDCliente]
      ,[Cliente]
      ,[Poliza]
      ,[PolizaExc]
      ,[Correo]
      ,[Vehiculos]
      ,[Asignacion]
      ,[Enviado]
      ,[FechaEnvio]
      ,[Envios]
      ,[SinAux]
      ,[Plantilla]
      ,[Fisica]
  FROM [FactuMex].[dbo].[RenovacionesBPD]
  WHERE	(IDRenovacion>4059 OR YEAR(FechaEnvio)=YEAR(GETDATE())) AND CUERPO IS NULL
";
        }
        public static string CartPendEnvio()
        {
            return $@"
SELECT [IDCliente]
      ,[Cliente]
      ,[Poliza]
      ,[PolizaExc]
      ,[Correo]
      ,[Vehiculos]
      ,[Asignacion]
      ,[Enviado]
      ,[FechaEnvio]
      ,[Envios]
      ,[SinAux]
      ,[Plantilla]
      ,[Fisica]
  FROM [FactuMex].[dbo].[RenovacionesBPD]
  WHERE	(IDRenovacion>4059 OR YEAR(FechaEnvio)=YEAR(GETDATE())) AND ENVIADO IS NULL OR Envios=0
";
        }
        public static string CartNotReady()
        {
            return $@"
SELECT [IDCliente]
      ,[Cliente]
      ,[Poliza]
      ,[PolizaExc]
      ,[Correo]
      ,[Vehiculos]
      ,[Asignacion]
      ,[Enviado]
      ,[FechaEnvio]
      ,[Envios]
      ,[SinAux]
      ,[Plantilla]
      ,[Fisica]
  FROM [FactuMex].[dbo].[RenovacionesBPD]
  WHERE	(IDRenovacion>4059 OR YEAR(FechaEnvio)=YEAR(GETDATE())) AND Ready2Go IS NULL OR Ready2Go=0
";
        }
        public static string StatReno()
        {
            return $@"
                        SELECT	(SELECT	COUNT(DISTINCT(POLIZA))
		                        FROM	[FactuMex].DBO.RenovacionesBPD) CLIDB,
		                        (SELECT	COUNT(DISTINCT(POLIZA))
		                        FROM	[FactuMex].DBO.RenovacionesBPD
		                        WHERE	YEAR(FechaEnvio)=YEAR(GETDATE())) CLIACT,
		                        (SELECT	COUNT(DISTINCT(POLIZA))
		                        FROM	[FactuMex].DBO.RenovacionesBPD
		                        WHERE	YEAR(FechaEnvio)=YEAR(GETDATE())-1) CLIANT,
		                        (SELECT	COUNT(DISTINCT(POLIZAEXC))
		                        FROM	[FactuMex].DBO.RenovacionesBPD
		                        WHERE	YEAR(FechaEnvio)=YEAR(GETDATE())) EXCACT,
		                        (SELECT	COUNT(DISTINCT(POLIZAEXC))
		                        FROM	[FactuMex].DBO.RenovacionesBPD
		                        WHERE	YEAR(FechaEnvio)=YEAR(GETDATE())-1) EXCANT,
		                        (SELECT	COUNT(*)
		                        FROM	[FactuMex].DBO.RenovacionesBPD
		                        WHERE	IDRenovacion>4059 OR YEAR(FechaEnvio)=YEAR(GETDATE())) SOLCACT,
		                        (SELECT	COUNT(*)
		                        FROM	[FactuMex].DBO.RenovacionesBPD
		                        WHERE	(IDRenovacion>4059 OR YEAR(FechaEnvio)=YEAR(GETDATE())) AND CUERPO IS NULL) PENDCUERPOACT,
		                        (SELECT	COUNT(*)
		                        FROM	[FactuMex].DBO.RenovacionesBPD
		                        WHERE	(IDRenovacion>4059 OR YEAR(FechaEnvio)=YEAR(GETDATE())) AND ENVIADO IS NULL OR Envios=0) PENDENVIOACT,
		                        (SELECT	COUNT(*)
		                        FROM	[FactuMex].DBO.RenovacionesBPD
		                        WHERE	(IDRenovacion>4059 OR YEAR(FechaEnvio)=YEAR(GETDATE())) AND Ready2Go IS NULL OR Ready2Go=0) NOTREADY,
                                (SELECT	COUNT(*)
		                         FROM	[FactuMex].DBO.RenovacionesBPD
		                         WHERE	FISICA=1 AND YEAR(FechaEnvio)=YEAR(GETDATE())) FISICAACT,
                                (SELECT	COUNT(*)
		                         FROM	[FactuMex].DBO.RenovacionesBPD
		                         WHERE	FISICA=1 AND YEAR(FechaEnvio)=YEAR(GETDATE())-1) FISICAANT
                    ";
        }

        internal static string ValidaEmail()
        {
            return "SELECT RTRIM(LTRIM(EMAIL)) EMAIL FROM TG_ENTE WHERE IDENTE=(SELECT TOP 1 IDCLIENTE FROM SG_OPERACION WHERE CODPOLIZA=@CODPOLIZA)";
        }

        public static string CargaAno()
        {
            return "SELECT TOP 1 VALOR FROM [FactuMex].[DBO].[PARAMETRO] WHERE PARAMETRO='ANO'";
        }

        public static string CargaAnexos()
        {
            return "SELECT IDANEXO-1 INDICE,NOMBRE FROM [FactuMex].[dbo].[Anexos]";
        }
        public static string CargaPlantillas()
        {
            return "SELECT * FROM [FactuMex].[dbo].[Plantillas]";
        }
        public static string CargaUsuariosxEmail()
        {
            return "SELECT TOP 1 [IDUsuario],[Nombre],[Email],':)' [Pwd],[Cedula],[Telefono],[Foto],[Borrado] FROM RenoUsuario WHERE EMAIL=@BUSCAR;";
        }
        public static string CargaUsuariosxBusqueda()
        {
            return "SELECT TOP 10 * FROM RenoUsuario WHERE NOMBRE LIKE '%'+@BUSCAR+'%' OR EMAIL LIKE '%'+@BUSCAR+'%' OR CEDULA LIKE '%'+@BUSCAR+'%' ORDER BY NOMBRE;";
        }

        public static string GetExisteAnoActual()
        {
            return $@"  DECLARE @EXISTE NVARCHAR(50);
                        IF(CHARINDEX('AUXS',@CODPOLIZA)>0)
                        BEGIN
                            SET @EXISTE=(SELECT TOP 1 'SOLYAEXISTE|'+@CODPOLIZA FROM [FactuMex].[DBO].[RENOVACIONESBPD] WHERE POLIZAEXC=@CODPOLIZA AND ANO=@ANO);
                        END
                        ELSE
                        BEGIN
                            SET @EXISTE=(SELECT TOP 1 'SOLYAEXISTE|'+@CODPOLIZA FROM [FactuMex].[DBO].[RENOVACIONESBPD] WHERE POLIZA=@CODPOLIZA AND ANO=@ANO);
                        END
                        IF(@EXISTE IS NULL)
                        BEGIN
                            SET @EXISTE='SOLNOEXISTE|'+@CODPOLIZA; 
                        END
                        SELECT  @EXISTE;";
        }

        public static string GetAU()
        {
            return "SELECT TOP 1 CODPOLIZA FROM SG_OPERACION WHERE CODPOLIZA LIKE 'AU-%' AND CODTIPOORDEN='RN' AND IDCLIENTE=(SELECT TOP 1 IDCLIENTE FROM [INBROKER].[DBO].[SG_OPERACION] WHERE CODPOLIZA=@CODPOLIZA AND CODTIPOORDEN='RN' ORDER BY IDOPERACION DESC) ORDER BY IDOPERACION DESC";
        }

        public static string CargaEnviosPendxNombre()
        {
            return "SELECT TOP 50 IDCLIENTE,CLIENTE,POLIZA,POLIZAEXC,CUERPO,(SELECT PLANTILLA FROM DBO.PLANTILLAS WHERE CODPLANTILLA=R.PLANTILLA) PLANTILLA,FISICA FROM RENOVACIONESBPD R WHERE (FISICA=0 OR FISICA IS NULL) AND ENVIOS=0 AND READY2GO=1 AND NOT CUERPO IS NULL AND CLIENTE LIKE '%'+@NOMBRE+'%' OR POLIZA=@NOMBRE OR POLIZAEXC=@NOMBRE;";
        }

        public static string CargaEnviosPend()
        {
            return "SELECT TOP 50 IDCLIENTE,CLIENTE,POLIZA,POLIZAEXC,CUERPO,(SELECT PLANTILLA FROM DBO.PLANTILLAS WHERE CODPLANTILLA=R.PLANTILLA) PLANTILLA,FISICA FROM RENOVACIONESBPD R WHERE  (FISICA=0 OR FISICA IS NULL) AND ENVIOS=0 AND READY2GO=1 AND NOT CUERPO IS NULL;";
        }

        public static string CargaUsuarios()
        {
            return "SELECT TOP 10 * FROM RenoUsuario WHERE BORRADO IS NULL ORDER BY NOMBRE;";
        }
        private static string SeleccionaCriterio(string Buscar)
        {
            string Criterio = $"BUSCARCOMO LIKE '%{Buscar}%'";
            string Orig = Buscar;
            Buscar = Buscar.Replace("-", "");
            if (long.TryParse(Buscar, out long NumBusqueda))
            {
                int lBusqueda = Buscar.Length;
                if (lBusqueda <= 6)
                {
                    return "IDCLIENTE=" + Buscar;
                }
                if (lBusqueda == 10)
                {
                    return $@"(TELEFONO1='{Buscar}' OR TELEFONO2='{Buscar}' OR TELEFONO3='{Buscar}' OR TELEFONO1='{Orig}' OR TELEFONO2='{Orig}' OR TELEFONO3='{Orig}')";
                }
                if (lBusqueda == 11)
                {
                    return $@"(NROFISCAL='{Buscar}' OR NROFISCAL='{Orig}')";
                }
            }
            else
            {
                if (Orig.Contains("AU-") | Orig.Contains("AUXS-"))
                    return $"IDCLIENTE=(SELECT TOP 1 IDCLIENTE FROM SG_OPERACION WHERE CODPOLIZA LIKE '{Orig}')";
            }
            return Criterio;
        }

        public static string GuardaConfigAno()
        {
            return "UPDATE dbo.Parametro SET VALOR=@ANO WHERE PARAMETRO='ANO'";
        }
        public static string GuardaAnexo()
        {
            return "UPDATE [FactuMex].[dbo].[Anexos] SET Nombre=@NOMBRE, Contenido=@CONTENIDO WHERE IDANEXO=@INDICE";
        }
        public static string GuardaPlantillaSel()
        {
            return $@"  INSERT  INTO [FactuMex].[dbo].[PlantillaxPoliza]
                        SELECT  @CODPOLIZA,@PLANTILLA,@FISICO; 
                        SELECT  @@IDENTITY";
        }
        public static string CargaUltOperacionRN()
        {
            return $@"SELECT TOP 1 IDOPERACION
		              FROM [Inbroker].[dbo].[SG_Operacion]
		              WHERE CODPOLIZA=@CODPOLIZA
                      AND	YEAR(FecVigenciaInicialPoliza)=(SELECT TOP 1  VALOR FROM [FactuMex].[dbo].[Parametro] WHERE Parametro='ANO')
                      AND	CodTipoOrden='RN'";
        }
        public static string BuscaCliente(string Buscar)
        {

            string Criterio = SeleccionaCriterio(Buscar);
            return $@"SELECT	TOP 30
		                IdEnte,
		                BuscarComo,
		                NroFiscal,
		                REPLACE	(TELEFONO1,'-','') TEL1,
		                REPLACE	(TELEFONO2,'-','') TEL2,
		                REPLACE	(TELEFONO3,'-','') TEL3,
		                EMAIL,
		                IDCLIENTE,
		                CODGRUPOEMPRESA,
		                (SELECT DESCGRUPOEMPRESA FROM Sg_GrupoEmpresa WHERE CODGRUPOEMPRESA=CL.CODGRUPOEMPRESA) GRUPOEMPRESA,
		                IDGRUPOECONOMICO,
		                (SELECT DESCGRUPOECONOMICO FROM SG_GRUPOECONOMICOCONSULTA WHERE IDENTEGRUPOECONOMICO=CL.IDGRUPOECONOMICO) GRUPOECONOMICO,
		                (SELECT DESCPUNTOVENTA FROM SG_PUNTOVENTA WHERE CODPUNTOVENTA=CL.CODPUNTOVENTA) PTOVTA,
		                CODCLIENTE,
		                (SELECT DESCSEGMENTACION FROM SG_SEGMENTACIONCARTERA WHERE CODSEGMENTACION=CL.CODSEGMENTACION1) SEG1,
		                (SELECT DESCSEGMENTACION FROM SG_SEGMENTACIONCARTERA WHERE CODSEGMENTACION=CL.CODSEGMENTACION2) SEG2,
		                (SELECT DESCSEGMENTACION FROM SG_SEGMENTACIONCARTERA WHERE CODSEGMENTACION=CL.CODSEGMENTACION3) SEG3,
		                (SELECT DESCSEGMENTACION FROM SG_SEGMENTACIONCARTERA WHERE CODSEGMENTACION=CL.CODSEGMENTACION4) SEG4,
		                (SELECT DESCSEGMENTACION FROM SG_SEGMENTACIONCARTERA WHERE CODSEGMENTACION=CL.CODSEGMENTACION5) SEG5,
		                (SELECT DESCSEGMENTACION FROM SG_SEGMENTACIONCARTERA WHERE CODSEGMENTACION=CL.CODSEGMENTACION6) SEG6,
		                (SELECT DESCSEGMENTACION FROM SG_SEGMENTACIONCARTERA WHERE CODSEGMENTACION=CL.CODSEGMENTACION7) SEG7,
		                (SELECT DESCSEGMENTACION FROM SG_SEGMENTACIONCARTERA WHERE CODSEGMENTACION=CL.CODSEGMENTACION8) SEG8,
		                SNVIP,
		                CL.ESTADO,
		                UPPER(CL.USUARIO) USUARIO_CLIENTE,
		                ISNULL((SELECT TOP 1 CODTIPOORDEN FROM SG_OPERACION WHERE IDCLIENTE=CL.IDCLIENTE AND ESTADO!='B' AND		CODPOLIZA LIKE 'AU-%' ORDER BY IDOPERACION DESC),'NOPOLIZA') POLVEH,
		                (SELECT TOP 1 IMPSUMAASEGURADAOPERACION FROM SG_OPERACION WHERE CODTIPOORDEN='RN' AND IDCLIENTE=CL.IDCLIENTE AND ESTADO!='B' AND		CODPOLIZA LIKE 'AU-%' ORDER BY IDOPERACION DESC) SUMASEGURADA,
		                (SELECT TOP 1 FECVIGENCIAINICIALPOLIZA FROM SG_OPERACION WHERE CODTIPOORDEN='RN' AND IDCLIENTE=CL.IDCLIENTE AND ESTADO!='B' AND		CODPOLIZA LIKE 'AU-%' ORDER BY IDOPERACION DESC) VIGINIAU,
		                (SELECT TOP 1 FECVIGENCIAFINALPOLIZA FROM SG_OPERACION WHERE CODTIPOORDEN='RN' AND IDCLIENTE=CL.IDCLIENTE AND ESTADO!='B' AND		CODPOLIZA LIKE 'AU-%' ORDER BY IDOPERACION DESC) VIGFINAU,
		                (SELECT TOP 1 FECVIGENCIAINICIALPOLIZA FROM SG_OPERACION WHERE CODTIPOORDEN='RN' AND IDCLIENTE=CL.IDCLIENTE AND ESTADO!='B' AND		CODPOLIZA LIKE 'AUXS-%' ORDER BY IDOPERACION DESC) VIGINIAUXS,
		                (SELECT TOP 1 FECVIGENCIAFINALPOLIZA FROM SG_OPERACION WHERE CODTIPOORDEN='RN' AND IDCLIENTE=CL.IDCLIENTE AND ESTADO!='B' AND		CODPOLIZA LIKE 'AUXS-%' ORDER BY IDOPERACION DESC) VIGFINAUXS,
		                (SELECT TOP 1 USUARIO FROM SG_OPERACION WHERE CODTIPOORDEN='RN' AND IDCLIENTE=CL.IDCLIENTE AND ESTADO!='B' AND		CODPOLIZA LIKE 'AU-%' ORDER BY IDOPERACION DESC) USUARIOOPEAU,
		                (SELECT TOP 1 USUARIO FROM SG_OPERACION WHERE CODTIPOORDEN='RN' AND IDCLIENTE=CL.IDCLIENTE AND ESTADO!='B' AND		CODPOLIZA LIKE 'AUXS-%' ORDER BY IDOPERACION DESC) USUARIOOPEAUXS,
		                ISNULL((SELECT	TOP 1 
				                IG.DESCITEM
		                FROM	SG_ITEMGENERAL IG

		                JOIN	SG_ITEMVEHICULO IV
		                ON		IV.IDMOVITEM=IG.IDMOVITEM
                        	                
                        WHERE	IDOPERACION IN
		                (SELECT	TOP 1 IDOPERACION 
		                FROM	SG_OPERACION 
		                WHERE	CODTIPOORDEN='RN' 
		                AND		CODPOLIZA LIKE 'AU-%'
		                AND		IDCLIENTE=CL.IDCLIENTE 
		                ORDER BY IDOPERACION DESC)
		                AND		CODVEHICULOUSO='006'),'SIN VEHICULO ASIGNADO') ASIGNADO,
		                ISNULL((SELECT	TOP 1 
				                IV.CHASIS
		                FROM	SG_ITEMGENERAL IG

		                JOIN	SG_ITEMVEHICULO IV
		                ON		IV.IDMOVITEM=IG.IDMOVITEM
                        
		                WHERE	IDOPERACION IN
		                (SELECT	TOP 1 IDOPERACION 
		                FROM	SG_OPERACION 
		                WHERE	CODTIPOORDEN='RN' 
		                AND		CODPOLIZA LIKE 'AU-%'
		                AND		IDCLIENTE=CL.IDCLIENTE 
		                ORDER BY IDOPERACION DESC)
		                AND		CODVEHICULOUSO='006'),'SIN VEHICULO ASIGNADO')  CHASIS,
		                ISNULL((		SELECT	DIC.VALORDETALLEITEMCOB ASIGNACION
		                FROM	SG_ITEMGENERAL IG

		                JOIN	SG_ITEMVEHICULO IV
		                ON		IV.IDMOVITEM=IG.IDMOVITEM
                        
		                JOIN	SG_DETALLEITEMCOBERTURA DIC
		                ON		DIC.IDMOVITEM=IG.IDMOVITEM
		                AND		DIC.CODTIPODETALLEITEMCOB='A3'
		                AND		DIC.ESTADOACCESORIO!='B'
		                AND		DIC.ESTADO!='B'

		                WHERE	IDOPERACION IN
		                (SELECT	TOP 1 IDOPERACION 
		                FROM	SG_OPERACION 
		                WHERE	CODTIPOORDEN='RN' 
		                AND		CODPOLIZA LIKE 'AU-%'
		                AND		IDCLIENTE=CL.IDCLIENTE
		                ORDER BY IDOPERACION DESC)
		                AND		CODVEHICULOUSO='006'),0) ASIGNACION
                FROM	TG_ENTE TG

                JOIN	SG_CLIENTE CL
                ON		CL.IDCLIENTE=TG.IdEnte


                WHERE	CL.IDGRUPOECONOMICO=22877
                AND		CL.ESTADO!='B'
                AND     {Criterio}
                ORDER	BY BUSCARCOMO";
        }

        public static string ActualizaUsuario()
        {
            return $@"  UPDATE  [dbo].[RenoUsuario]
                        SET     NOMBRE=@NOMBRE,
                                EMAIL=@EMAIL,
                                PWD=@PWD,
                                CEDULA=@CEDULA,
                                TELEFONO=@TELEFONO,
                                FOTO=@FOTO
                        WHERE   IDUSUARIO=@IDUSUARIO;
                        SELECT  @IDUSUARIO;";
        }

        public static string GuardaUsuario()
        {
            return $@"  INSERT  INTO  [dbo].[RenoUsuario]
                        SELECT  @NOMBRE,
                                @EMAIL,
                                @PWD,
                                @CEDULA,
                                @TELEFONO,
                                @FOTO,
                                NULL;
                        SELECT  @@IDENTITY;";
        }

        public static string BorraRelOrigenDestino()
        {
            return $@"DELETE FROM [dbo].[RelOrigenDest] WHERE PLANTILLA=@PLANTILLA";
        }



        public static string CargaRelOrigenDestino()
        {
            return $@"SELECT * FROM [dbo].[RelOrigenDest]";
        }

        public static string CargaOperacion(string IDCliente)
        {
            return $@"SELECT	IDOPERACION,
		            CASE 
			            WHEN CODOPERACIONESTADO=1 THEN 'VIGENTE'
			            WHEN CODOPERACIONESTADO=2 THEN 'NO VIGENTE'
			            WHEN CODOPERACIONESTADO=3 THEN 'CANCELADA'
			            WHEN CODOPERACIONESTADO=4 THEN 'RENOVADA'
			            WHEN CODOPERACIONESTADO=5 THEN 'CANCELADA'
		            END OPERACIONESTADO,
		            (SELECT TOP 1 DESCRAMO FROM SG_RAMO WHERE CODRAMO IN(SELECT TOP 1 CODRAMO FROM SG_SECCION WHERE CODSECCION=O.CODSECCION)) RAMO,
		            (SELECT TOP 1 DESCSECCION FROM SG_SECCION WHERE CODSECCION=O.CODSECCION) SECCION,
		            IDPOLIZA,
		            CODPOLIZA,
		            FECEMISION,
		            FECVIGENCIAINICIALPOLIZA FECVIGINI,
		            FECVIGENCIAFINALPOLIZA FECVIGFIN,
		            FECFACTURACION,
		            CASE WHEN SNFACTURADA='S' THEN 'FACTURADA' ELSE 'PEND FACTURAR' END FACT,
		            CODFACTURAASEGURADORA CODFACT,
		            IMPSUMAASEGURADAOPERACION SUMAASEGURADA,
		            OBSPOLIZA,
		            DP_IMPPREMIOIVA*TIPOCAMBIOOPERACION PRIMARD,
		            CASE WHEN SNULTIMOESTADO='S' THEN 'ULT ESTADO' ELSE 'MOV POSTERIORES' END ULTESTADO,
		            (SELECT DESCSEGMENTACION FROM SG_SEGMENTACIONCARTERA WHERE CODSEGMENTACION=O.CODSEGMENTACION1) GESTOR,
		            (SELECT DESCSEGMENTACION FROM SG_SEGMENTACIONCARTERA WHERE CODSEGMENTACION=O.CODSEGMENTACION2) DEPARTAMENTO,
		            (SELECT DESCSEGMENTACION FROM SG_SEGMENTACIONCARTERA WHERE CODSEGMENTACION=O.CODSEGMENTACION3) GERENTE,
		            (SELECT DESCSEGMENTACION FROM SG_SEGMENTACIONCARTERA WHERE CODSEGMENTACION=O.CODSEGMENTACION4) EJECUTIVO,
		            (SELECT DESCSEGMENTACION FROM SG_SEGMENTACIONCARTERA WHERE CODSEGMENTACION=O.CODSEGMENTACION5) CORRESPONSAL,
		            (SELECT DESCSEGMENTACION FROM SG_SEGMENTACIONCARTERA WHERE CODSEGMENTACION=O.CODSEGMENTACION6) COBROS,
		            (SELECT DESCSEGMENTACION FROM SG_SEGMENTACIONCARTERA WHERE CODSEGMENTACION=O.CODSEGMENTACION7) RECLAMACION,
		            (SELECT DESCSEGMENTACION FROM SG_SEGMENTACIONCARTERA WHERE CODSEGMENTACION=O.CODSEGMENTACION8) SUCURSAL,
		            SNSINIESTRADA,
		            FECALTA,
		            FECMODIFICACION,
		            ESTADO,
		            USUARIO

            FROM	SG_OPERACION O
            WHERE	IDCLIENTE={IDCliente}
            AND		CODTIPOORDEN='RN'
            AND		CODPOLIZA LIKE 'AU-%'
            ORDER	BY IDOPERACION DESC";
        }

        public static string IniciaSesion()
        {
            return $@"  UPDATE SESION SET EXPIRA=GETDATE() WHERE IDUSUARIO=(SELECT TOP 1 IDUSUARIO FROM RENOUSUARIO WHERE EMAIL=@EMAIL) AND EXPIRA>GETDATE();
                        INSERT  INTO SESION
                        SELECT  @CODSESION,
                                (SELECT TOP 1 IDUSUARIO FROM RENOUSUARIO WHERE EMAIL=@EMAIL),
                                @EXPIRA,
                                @META;
                    
                        SELECT  @@IDENTITY";
        }
        public static string RetornaSesion()
        {
            return $@"SELECT  IDSESION,CODSESION,IDUSUARIO,EXPIRA, NULL BFP FROM SESION WHERE IDSESION=@IDSESION;";
        }
        public static string VerificaUsuario()
        {
            return "SELECT TOP 1 ISNULL('SI','NO') FROM RENOUSUARIO WHERE EMAIL=@EMAIL AND PWD=@PWD;";
        }
        public static string ValidaUsuario()
        {
            return $@"  DECLARE @R INT; 
                        SET @R=(SELECT TOP 1 ISNULL(0,1) FROM SESION WHERE CODSESION=@TOKEN AND BFP=@META AND EXPIRA>GETDATE()); 
                        IF(@R=1)
                        BEGIN 
                            UPDATE  SESION
                            SET     EXPIRA=GETDATE()
                            WHERE   CODSESION=@TOKEN;
                        END  
                        SELECT @R;";
        }
        public static string CargaVehiculos(string IDOperacion)
        {
            return $@"  SELECT	CASE WHEN IV.CodVehiculoUso='006' THEN 'ASIGNADO' ELSE 'OTRO' END USO,
                        ISNULL((SELECT  TOP 1 VALORDETALLEITEMCOB FROM Sg_DetalleItemCobertura WHERE IDMOVITEM = IG.IDMOVITEM AND ESTADO != 'B' AND ESTADOACCESORIO != 'B' AND CODTIPODETALLEITEMCOB = 'A3' ORDER BY IDDETALLEITEMCOBERTURA DESC),0) ASIGNACION,
		                IG.NROITEM,
		                IG.IDMOVITEM,
		                IG.IDITEM,
		                IG.DESCITEM,
		                (SELECT DESCCOBERTURA FROM SG_COBERTURA WHERE CODCOBERTURA = IG.CODCOBERTURA) COBERTURA,
		                IG.ANIOSSINSINIESTROS,
		                IG.FECALTA,
		                IG.ESTADO,
		                IG.USUARIO,
		                IV.PATENTE PLACA,
                        IV.CHASIS,
		                IV.Sn0Kilometro NUEVO,
                        IV.ANIOFABRICACION ANO,
                        VE.CodVehiculoTipo
                FROM    SG_ITEMGENERAL IG

                JOIN    SG_ITEMVEHICULO IV
                ON      IV.IDMOVITEM = IG.IDMOVITEM
                        
                JOIN	Sg_Vehiculo VE
                ON		VE.IDVEHICULO=IV.IDVEHICULO

                WHERE IG.IDOPERACION = {IDOperacion}";
        }

        public static string CargaPermisosPlantilla()
        {
            return $@"SELECT * FROM LSTPERMISO";
        }

        public static string GuardaPermisosUsuario()
        {
            return $@"
            IF(@ESTATUS='true')
            BEGIN
                INSERT  INTO DBO.PERMISOSXUSUARIO
                SELECT  @IDUSUARIO, @PERMISO
            END
            ELSE
            BEGIN
                DELETE
                FROM    DBO.PERMISOSXUSUARIO
                WHERE   IDUSUARIO=@IDUSUARIO
                AND     PERMISO=@PERMISO
            END;";
        }

        public static string CargaPermisosUsuario()
        {
            return $@"SELECT * FROM PERMISOSXUSUARIO WHERE IDUSUARIO=@IDUSUARIO";
        }

        public static string CargaPermisosxEmail()
        {
            return "SELECT PERMISO FROM PERMISOSXUSUARIO WHERE IDUSUARIO=(SELECT TOP 1 IDUSUARIO FROM RENOUSUARIO WHERE EMAIL=@EMAIL);";
        }

        public static string ValidaRenoExiste(string CodPoliza)
        {
            return $@"  DECLARE @CODPOLIZA NVARCHAR(50)
                        SET @CODPOLIZA='{CodPoliza}'
                        IF(CHARINDEX('AUXS',@CODPOLIZA)>0)
                        BEGIN
                            SELECT	  ISNULL((SELECT TOP 1 'SI' 
		                    FROM [FactuMex].[dbo].[RenovacionesBPD]
		                    WHERE POLIZA=(SELECT TOP 1 AU FROM [FactuMex].[dbo].[AUXSXAU] WHERE AUXS=@CODPOLIZA)),'NO') EXISTE
                        END
                        ELSE
                        BEGIN
                            SELECT	  ISNULL((SELECT TOP 1 'SI' 
		                    FROM [FactuMex].[dbo].[RenovacionesBPD]
		                    WHERE POLIZA='{CodPoliza}'),'NO') EXISTE
                        END";
                        
        }
        public static string GetAnoActual()
        {
            return "SELECT TOP 1  VALOR FROM [FactuMex].[dbo].[Parametro] WHERE Parametro='ANO';";
        }
        public static string GetPlantillaxCodPoliza()
        { 
        return $@"
                 DECLARE @PLANTILLA NVARCHAR(50);
                            SET     @PLANTILLA=ISNULL((SELECT TOP 1 UPPER(PLANTILLA)
		                              FROM [FactuMex].[dbo].[PlantillaxPoliza]
		                              WHERE POLIZA=@CODPOLIZA OR POLIZA=(SELECT TOP 1 AU FROM [FactuMex].[DBO].[AuxsxAU] WHERE AUXS=@CODPOLIZA)),'SINPLANTILLA')   ;
                            SELECT  @PLANTILLA;
                ";
        }
        public static string ValidaRenoExisteParametrizada(String Ano,string Plantilla,string Existe, string AU)
        {
            return $@"  DECLARE @EXISTEPOL NVARCHAR(50);
                        SET     @EXISTEPOL='{Existe}';
                        
                        IF(@EXISTEPOL!='SOLYAEXISTE|'+@CODPOLIZA)
                        BEGIN

                            DECLARE @PLANTILLA NVARCHAR(50);
                            SET     @PLANTILLA='{Plantilla}';
                        
                            DECLARE   @IDOPERACION BIGINT
                            SET       @IDOPERACION=
                            (SELECT	  ISNULL((SELECT TOP 1 IDOPERACION
		                              FROM [Inbroker].[dbo].[SG_Operacion]
		                              WHERE CODPOLIZA=@CODPOLIZA
                                      AND	YEAR(FecVigenciaInicialPoliza)={Ano}
                                      AND	CodTipoOrden='RN'),0) EXISTE)
                            
                            
                            
                            IF(@IDOPERACION>0)
                            BEGIN
                            IF(CHARINDEX('AUXS',@CODPOLIZA)>0)
                            BEGIN
                            SET       @IDOPERACION=
                            (SELECT	  ISNULL((SELECT TOP 1 IDOPERACION
		                              FROM [Inbroker].[dbo].[SG_Operacion]
		                              WHERE CODPOLIZA='{AU}'
                                      AND	YEAR(FecVigenciaInicialPoliza)={Ano}
                                      AND	CodTipoOrden='RN'),0) EXISTE) 
                            END



                                IF(@PLANTILLA!='SINPLANTILLA')
                                BEGIN
                                    IF(@PLANTILLA='CONASIGNACION' OR @PLANTILLA='CONASIGNACIONSAUXS')
                                    BEGIN
                                        DECLARE	@IDMOVITEMASIGNADO BIGINT
                                        SET		@IDMOVITEMASIGNADO=
                                        ISNULL((SELECT	TOP 1 IV.IDMOVITEM
                                        FROM	[Inbroker].[dbo].SG_ITEMGENERAL IG

                                        JOIN	[Inbroker].[dbo].SG_ITEMVEHICULO IV
                                        ON		IV.IdMovItem=IG.IdMovItem
                                        AND		IV.CodVehiculoUso='006'
                        
                                        JOIN	[Inbroker].[dbo].[Sg_Vehiculo] VE
                                        ON		VE.IDVEHICULO=IV.IDVEHICULO

                                        WHERE	IDOPERACION=@IDOPERACION
                                        ORDER   BY IG.IDMOVITEM DESC),0)
                                    
                                        IF(@IDMOVITEMASIGNADO>0)
                                        BEGIN
	                                        DECLARE @MONTOASIGNADO FLOAT
	                                        SET		@MONTOASIGNADO=
	                                        ISNULL((SELECT	TOP 1 ValorDetalleItemCob
	                                        FROM	[Inbroker].[dbo].[Sg_DetalleItemCobertura]
	                                        WHERE	IDMOVITEM=@IDMOVITEMASIGNADO
                                            ORDER   BY IDMOVITEM DESC),0)
	                                        IF(@MONTOASIGNADO=0)
	                                        BEGIN
		                                        SELECT 'SINMONTOASIGNADO|'+@CODPOLIZA
	                                        END
                                            ELSE
                                            BEGIN
                                                DECLARE	@SUMA_ASEG FLOAT
                                                SET		@SUMA_ASEG=(SELECT TOP 1 ISNULL(ImpSumaAsegurada,0) FROM [Inbroker].[dbo].[Sg_ItemGeneral] WHERE IdMovItem=@IDMOVITEMASIGNADO)
                                                IF(@SUMA_ASEG=0)
	                                            BEGIN
		                                            SELECT 'SINSUMAASEGURADA|'+@CODPOLIZA
	                                            END
                                                ELSE
                                                BEGIN
                                                    SELECT 'TODOBIEN|'+@CODPOLIZA
                                                END
                                            END
                                        END
                                        ELSE
                                        BEGIN
	                                        SELECT 'SINVEHICULOASIGNADO|'+@CODPOLIZA
                                        END
                                    END
                                    ELSE
                                    BEGIN
                                        SELECT 'TODOBIEN|'+@CODPOLIZA
                                    END
                                END
                                ELSE
                                BEGIN
                                    SELECT 'SINPLANTILLA|'+@CODPOLIZA;
                                END
                            END
                            ELSE
                            BEGIN
                                SELECT 'NOEXISTE|'+@CODPOLIZA;
                            END
                            
                        END
                        ELSE
                        BEGIN
                            SELECT  @EXISTEPOL;
                        END";
        }
        public static string GuardaAU()
        {
            return "UPDATE [FactuMex].[dbo].[RenovacionesBPD] SET	AT1NAME=@FILENAME, AT1=@FILEBYTES WHERE	POLIZA=@CODPOLIZA";
        }

        public static string CargaAsignacion()
        {
            return $@"
                        --DETERMINANDO EL VEHICULO ASIGNADO DE MAYOR VALOR
                        DECLARE @IDMOVITEM BIGINT
                        SET		@IDMOVITEM=
                        (SELECT	TOP 1 IG.IdMovItem
                        FROM	SG_ITEMGENERAL IG

                        JOIN	SG_ITEMVEHICULO IV
                        ON		IV.IDMOVITEM=IG.IDMOVITEM
                        AND		IV.CODVEHICULOUSO='006'
                        
                        WHERE	IG.ESTADO!='B'
                        AND		IDOPERACION IN
                        (SELECT	IDOPERACION
                        FROM	SG_OPERACION
                        WHERE	ESTADO!='B'
                        AND		CODPOLIZA LIKE 'AU-%'
                        AND		CODTIPOORDEN='RN'
                        AND		CODOPERACIONESTADO='1'
                        AND		IDCLIENTE IN
                        (SELECT	C.IDCLIENTE
                        FROM	[Inbroker].[dbo].[Tg_Ente] E

                        JOIN	[Inbroker].[dbo].[Sg_Cliente] C
                        ON		C.IdCliente=E.IdEnte
                        AND		C.Estado!='B'

                        WHERE	(NroFiscal=@CEDULA OR NroFiscal=@CEDULASINGUI)
                        AND		E.Estado!='B'))
                        ORDER	BY IG.ImpSumaAsegurada DESC)
                         IF(NOT @IDMOVITEM IS NULL)
                        BEGIN
                            --ELIMINANDO REGISTROS PREVIOS DE ASIGNACIONES
                            DELETE
                            FROM	Sg_DetalleItemCobertura
                            WHERE	IdMovItem=@IDMOVITEM

                            --GUARDANDO ASIGNACION
                            INSERT	INTO Sg_DetalleItemCobertura
                            SELECT	(SELECT MAX(IDDETALLEITEMCOBERTURA) FROM Sg_DetalleItemCobertura)+1,
		                            '00000001-I',
		                            @IDMOVITEM,
		                            NULL,
		                            'A3',
		                            'ASIGNACION BPD',
		                            @VALOR,
		                            NULL,
		                            'A',
		                            GETDATE(),
		                            GETDATE(),
		                            NULL,
		                            'A',
		                            'SA',
		                            NEWID();

                            UPDATE	[Inbroker].[dbo].[Iw_Numeracion]
                            SET		UltimoNumero=(SELECT MAX(IDDETALLEITEMCOBERTURA)+1 FROM SG_DETALLEITEMCOBERTURA)
                            WHERE	DescTabla LIKE 'SG_DETALLEITEMCOBERTURA'
                            AND		DescAtributo='IDDETALLEITEMCOBERTURA';
                        END
                        ";
        }

        public static string ActualizaEnte(string rels, string tabla, string cond)
        {
            return $@"
                    UPDATE	[Inbroker].[DBO].[TG_ENTE]
                    SET		{rels}
                    FROM	[FactuMex].[DBO].[{tabla}]
                    WHERE	[Inbroker].[DBO].[TG_ENTE].NROFISCAL=[FactuMex].[DBO].[{tabla}].{cond}
                    ";
        }

        public static string GuardaAUXS()
        {
            return "UPDATE [FactuMex].[dbo].[RenovacionesBPD] SET	AT2NAME=@FILENAME, AT2=@FILEBYTES WHERE	POLIZA=@CODPOLIZA";
        }
        public static string EnviaMail()
        {
            return $@"  UPDATE	DBO.RENOVACIONESBPD SET	ENVIADO=NULL WHERE POLIZA=@CODPOLIZA;
                        SELECT RUTA FROM DBO.RENOVACIONESBPD WHERE POLIZA=@CODPOLIZA AND ENVIADO IS NULL ORDER BY IDRENOVACION DESC; 
                    ";
        }
        public static string CargaCorreo()
        {
            return "SELECT TOP 1 [Cuerpo] FROM[FactuMex].[dbo].[RenovacionesBPD] WHERE POLIZA=@CODPOLIZA  ORDER BY IDRENOVACION DESC";
        }
        public static string GuardaArchivo()
        {
            return $@"  INSERT  INTO dbo.Importar
                        SELECT  NULL,
		                        @RUTA,
		                        @NOMBRE,
		                        @FECHA,
		                        NULL";
        }
        public static string CargaArchivo()
        {
            return $@"SELECT  * from dbo.Importar";
        }
        public static string GuardaRelOrigenDestino()
        {
            return $@"  INSERT INTO [dbo].[RelOrigenDest]
                        SELECT	   @ORIGEN,
                                   @DESTINO,
                                   @PLANTILLA";
        }
        public static string GuardaAsignacion()
        {
            return $@"
                        UPDATE	SG_ITEMVEHICULO
                        SET		CODVEHICULOUSO='001'
                        WHERE	IDMOVITEM IN
                        (SELECT	IV.IDMOVITEM
                        FROM	SG_ITEMGENERAL IG

                        JOIN	SG_ITEMVEHICULO IV
                        ON		IV.IDMOVITEM=IG.IDMOVITEM
                        AND		CODVEHICULOUSO='006'

                        WHERE	IDOPERACION IN
                        (SELECT	IDOPERACION
                        FROM	SG_ITEMGENERAL
                        WHERE	IDMOVITEM=@IDMOVITEM))

                        UPDATE	SG_ITEMVEHICULO
                        SET		CODVEHICULOUSO='006'
                        WHERE	IDMOVITEM=@IDMOVITEM

                        DELETE
                        FROM	[Inbroker].[dbo].[Sg_DetalleItemCobertura]
                        WHERE	IDMOVITEM IN
                        (SELECT	IV.IDMOVITEM
                        FROM	SG_ITEMGENERAL IG

                        JOIN	SG_ITEMVEHICULO IV
                        ON		IV.IDMOVITEM=IG.IDMOVITEM
                        AND		CODVEHICULOUSO='006'

                        WHERE	IDOPERACION IN
                        (SELECT	IDOPERACION
                        FROM	SG_ITEMGENERAL
                        WHERE	IDMOVITEM=@IDMOVITEM))

                        INSERT	INTO Sg_DetalleItemCobertura
                        SELECT	(SELECT MAX(IDDETALLEITEMCOBERTURA) FROM Sg_DetalleItemCobertura)+1,
                                '00000001-I',
                                @IDMOVITEM,
                                NULL,
                                'A3',
                                'ASIGNACION BPD',
                                @ASIGNACION,
		                        NULL,
		                        'A',
		                        GETDATE(),
		                        GETDATE(),
		                        NULL,
		                        'A',
		                        'SA',
		                        NEWID()

                        SELECT  @@IDENTITY
                    ";
        }
    }
}

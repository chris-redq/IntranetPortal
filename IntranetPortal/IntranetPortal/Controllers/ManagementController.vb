﻿Imports System.Net
Imports System.Web.Http
Imports System.Web.Http.Description
Imports Newtonsoft.Json.Linq
Imports IntranetPortal.Data
Imports IntranetPortal.Data.RulesEngine

Namespace Controllers
    Public Class ManagementController
        Inherits ApiController

        Private ruleEngineName = System.Configuration.ConfigurationManager.AppSettings("RulesEngineServer")

        <ResponseType(GetType(BaseRule()))>
        <Route("api/Management/RulesEngine")>
        Function GetRulesInEngine() As BaseRule()
            Using svr As New RulesEngineServices(ruleEngineName)
                Return svr.Rules
            End Using

        End Function


    End Class
End Namespace
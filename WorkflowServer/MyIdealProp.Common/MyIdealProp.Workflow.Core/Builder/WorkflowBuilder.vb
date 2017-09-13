Imports System.Collections.Generic
Imports MyIdealProp.Workflow.Core.Cache
Imports MyIdealProp.Workflow.Core.Fault
Imports MyIdealProp.Workflow.Core.Model
Imports MyIdealProp.Workflow.Core.Parser
Imports System

Namespace MyIdealProp.Workflow.Core.Builder

    Friend NotInheritable Class WorkflowBuilder(Of TSchemeMedium As Class)
        Implements IWorkflowBuilder

        Friend Parser As IWorkflowParser(Of TSchemeMedium)

        Friend wfDataBase As New Data.WorkflowDatabase

        Private _haveCache As Boolean

        Private _cache As IParsedProcessCache

        Friend Sub New()

        End Sub

        Public Sub New(parser__2 As IWorkflowParser(Of TSchemeMedium))
            Parser = parser__2
        End Sub

        Public Function GetProcessScheme(processName As String) As ProcessDefinition Implements IWorkflowBuilder.GetProcessScheme
            Return GetProcessScheme(processName, New Dictionary(Of String, IEnumerable(Of Object))())
        End Function

        Public Function GetProcessScheme(processName As String, parameters As IDictionary(Of String, IEnumerable(Of Object))) As ProcessDefinition Implements IWorkflowBuilder.GetProcessScheme
            Try
                Return GetProcessDefinition(CTypeDynamic(Of SchemeDefinition(Of TSchemeMedium))(wfDataBase.GetProcessSchemeByName(processName)))
            Catch generatedExceptionName As SchemeNotFoundException
                'Return GetProcessDefinition(CreateNewScheme(processName, parameters))
                Throw generatedExceptionName
            End Try
        End Function

        Public Function CreateNewProcess(processId As Integer, processName As String, parameters As IDictionary(Of String, IEnumerable(Of Object))) As ProcessInstance Implements IWorkflowBuilder.CreateNewProcess

            Dim schemeDefinition As SchemeDefinition(Of TSchemeMedium) = Nothing
            Try
                schemeDefinition = CTypeDynamic(Of SchemeDefinition(Of TSchemeMedium))(wfDataBase.GetProcessSchemeByName(processName))
            Catch generatedExceptionName As SchemeNotFoundException
                schemeDefinition = CreateNewScheme(processName, parameters)
            End Try

            Return ProcessInstance.Create(schemeDefinition.Id, GetProcessDefinition(schemeDefinition), schemeDefinition.IsObsolete, schemeDefinition.IsDeterminingParametersChanged)
        End Function

        Private Function CreateNewScheme(processName As String, parameters As IDictionary(Of String, IEnumerable(Of Object))) As SchemeDefinition(Of TSchemeMedium)
            Throw New NotImplementedException
        End Function

        Private Function GetProcessDefinition(schemeDefinition As SchemeDefinition(Of TSchemeMedium)) As ProcessDefinition
            If _haveCache Then
                Dim cachedDefinition = _cache.GetProcessDefinitionBySchemeId(schemeDefinition.Id)
                If cachedDefinition IsNot Nothing Then
                    Return cachedDefinition
                End If
                Dim processDefinition = Parser.Parse(CTypeDynamic(Of TSchemeMedium)(schemeDefinition.Scheme))
                _cache.AddProcessDefinition(schemeDefinition.Id, processDefinition)
                Return processDefinition
            End If

            Return Parser.Parse(CTypeDynamic(Of TSchemeMedium)(schemeDefinition.Scheme))
        End Function

        Public Function GetProcessInstance(processId As Integer) As ProcessInstance Implements IWorkflowBuilder.GetProcessInstance
            Dim schemeDefinition = wfDataBase.GetProcessSchemeById(processId)
            Return ProcessInstance.Create(schemeDefinition.Id, GetProcessDefinition(CTypeDynamic(Of SchemeDefinition(Of TSchemeMedium))(schemeDefinition)), schemeDefinition.IsObsolete, schemeDefinition.IsDeterminingParametersChanged)
        End Function

        Public Sub SetCache(cache As IParsedProcessCache) Implements IWorkflowBuilder.SetCache
            _cache = cache
            _haveCache = True
        End Sub

        Public Sub RemoveCache() Implements IWorkflowBuilder.RemoveCache
            _haveCache = False
            _cache.Clear()
            _cache = Nothing
        End Sub

        Public Function GetParser() As IWorkflowParser(Of TSchemeMedium)
            Return Parser
        End Function

        Public Function GetAllProcessScheme() As List(Of ProcessDefinition) Implements IWorkflowBuilder.GetAllProcessScheme
            Dim schemes = wfDataBase.GetAllProcess()
            Return schemes.Select(Function(sc) GetProcessDefinition(CTypeDynamic(Of SchemeDefinition(Of TSchemeMedium))(sc))).ToList
        End Function

        Public Function CreateNewProcessScheme(processId As Integer, processName As String, parameters As IDictionary(Of String, IEnumerable(Of Object))) As ProcessInstance Implements IWorkflowBuilder.CreateNewProcessScheme
            Throw New NotImplementedException
        End Function
    End Class
End Namespace

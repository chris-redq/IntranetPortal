Imports MyIdealProp.Workflow.Core.Model
Imports MyIdealProp.Workflow.Core
Imports MyIdealProp.Workflow.Core.Persistence

Public Class SchemePersistenceProvider
    Implements Persistence.ISchemePersistenceProvider(Of XElement)

    Private isDebug As Boolean

    Public Function GetProcessSchemeByProcessId(procInstId As Guid) As SchemeDefinition(Of XElement) Implements ISchemePersistenceProvider(Of XElement).GetProcessSchemeByProcessId
        Throw New NotImplementedException
    End Function

    Public Function GetProcessSchemeBySchemeId(schemeId As Guid) As SchemeDefinition(Of XElement) Implements ISchemePersistenceProvider(Of XElement).GetProcessSchemeBySchemeId
        Dim ps = ProcessScheme.GetSchemeById(schemeId)
        Return New SchemeDefinition(Of XElement)(0, ps.XScheme, False)
    End Function

    Public Function GetProcessSchemeWithParameters(processName As String, parameters As IDictionary(Of String, IEnumerable(Of Object))) As SchemeDefinition(Of XElement) Implements ISchemePersistenceProvider(Of XElement).GetProcessSchemeWithParameters
        Throw New Exception("GetProcessSchemeWithParameters")
    End Function

    Public Function GetProcessSchemeWithParameters(processName As String, parameters As IDictionary(Of String, IEnumerable(Of Object)), ignoreObsolete As Boolean) As SchemeDefinition(Of XElement) Implements ISchemePersistenceProvider(Of XElement).GetProcessSchemeWithParameters
        Throw New NotImplementedException
    End Function

    Public Sub SaveScheme(processName As String, schemeId As Guid, scheme As XElement, parameters As IDictionary(Of String, IEnumerable(Of Object))) Implements ISchemePersistenceProvider(Of XElement).SaveScheme
        Throw New NotImplementedException
    End Sub

    Public Function GetProcessSchemeByProcessName(processName As String) As SchemeDefinition(Of XElement) Implements ISchemePersistenceProvider(Of XElement).GetProcessSchemeByProcessName
        'Dim ps = ProcessScheme.GetSchemeByName(processName)
        'If ps Is Nothing Then
        '    Return Nothing
        'End If
        'Return New SchemeDefinition(Of XElement)(ps.ProcessId, ps.XScheme, False)
    End Function

    Public Function GetAllScheme() As List(Of SchemeDefinition(Of XElement)) Implements ISchemePersistenceProvider(Of XElement).GetAllScheme
        'Return ProcessScheme.GetAllScheme.Select(Function(ps) New SchemeDefinition(Of XElement)(ps.ProcessId, ps.XScheme, False)).ToList
    End Function
End Class

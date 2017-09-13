Imports System.Collections.Generic
Imports MyIdealProp.Workflow.Core.Model
Imports System

Namespace MyIdealProp.Workflow.Core.Cache
    'TODO Multithread
    Friend NotInheritable Class DefaultParcedProcessCache
        Implements IParsedProcessCache
        Private _cache As Dictionary(Of Integer, ProcessDefinition)

        Public Sub Clear() Implements IParsedProcessCache.Clear
            _cache.Clear()
        End Sub

        Public Function GetProcessDefinitionBySchemeId(schemeId As Integer) As ProcessDefinition Implements IParsedProcessCache.GetProcessDefinitionBySchemeId
            If _cache Is Nothing Then
                Return Nothing
            End If
            If _cache.ContainsKey(schemeId) Then
                Return _cache(schemeId)
            End If
            Return Nothing
        End Function

        Public Sub AddProcessDefinition(schemeId As Integer, processDefinition As ProcessDefinition) Implements IParsedProcessCache.AddProcessDefinition
            If _cache Is Nothing Then
                _cache = New Dictionary(Of Integer, ProcessDefinition)() From { _
                    {schemeId, processDefinition} _
                }
            Else
                If _cache.ContainsKey(schemeId) Then
                    _cache(schemeId) = processDefinition
                Else
                    _cache.Add(schemeId, processDefinition)
                End If
            End If
        End Sub
    End Class
End Namespace

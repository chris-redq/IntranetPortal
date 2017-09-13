Imports System.Collections.Generic

Namespace MyIdealProp.Workflow.Core.Model
    <Serializable>
    Public Class EventDefinition2
        Inherits BaseDefinition
        Public Property InputParameters As Dictionary(Of String, DataFieldDefinition)

        Public Shared Function Create(name As String) As EventDefinition2
            Return New EventDefinition2() With { _
                .Name = name, _
                .InputParameters = New Dictionary(Of String, DataFieldDefinition)() _
            }
        End Function

        Public Sub AddParameterRef(name As String, parameter As DataFieldDefinition)
            InputParameters.Add(name, parameter)
        End Sub
    End Class

End Namespace

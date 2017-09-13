Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization

Namespace MyIdealProp.Workflow.Core.Model
    <Serializable()>
     <DataContract()>
    Public MustInherit Class BaseDefinition
        <DataMember>
        Public Overridable Property Name() As String

        Protected Sub New()
        End Sub
    End Class
End Namespace

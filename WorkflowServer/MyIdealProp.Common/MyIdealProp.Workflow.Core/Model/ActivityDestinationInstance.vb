Imports System.Runtime.Serialization

Namespace MyIdealProp.Workflow.Core.Model

    <Serializable>
    <DataContract>
    Public Class ActivityDestinationInstance
        <DataMember>
        Public Property Id As Integer
        Public Property ProcessInstance As ProcessInstance
        Public Property ActivityInstance As ActivityInstance
        Public Property EventInstanceLog As List(Of EventInstance)
        <DataMember>
        Public Property DestinationUser As String
        <DataMember>
        Public Property StartTime As DateTime
        <DataMember>
        Public Property Status As ActivityStatus
        Friend Property IsClient As Boolean
        Friend Property ClientEventInst As EventInstance

        Private _actName As String
        <DataMember>
        Public Property ActivityName As String
            Get
                If String.IsNullOrEmpty(_actName) AndAlso ActivityInstance IsNot Nothing Then
                    _actName = ActivityInstance.ActivityName
                End If

                Return _actName
            End Get
            Set(value As String)
                _actName = value
            End Set
        End Property

        Public Overloads Shared Function Create(actInst As ActivityInstance, destUser As String) As ActivityDestinationInstance
            Dim actDestInst = New ActivityDestinationInstance With {
                .ProcessInstance = actInst.ProcessInstance,
                .ActivityInstance = actInst,
                .DestinationUser = destUser,
                .EventInstanceLog = New List(Of EventInstance),
                .StartTime = DateTime.Now,
                .Status = ActivityStatus.Running
                }

            actDestInst.AddParameters(actInst.ActivityDefinition.DataFields.Select(Function(df) DataFieldDefinitionWithValue.CreateWithDefaultValue(df)))

            Return actDestInst
        End Function

        <DataMember>
        Public Property DataFields() As IEnumerable(Of DataFieldDefinitionWithValue)
            Get
                Return _parameters
            End Get
            Friend Set(value As IEnumerable(Of DataFieldDefinitionWithValue))
                _parameters.Clear()
                _parameters.AddRange(value)
            End Set
        End Property
        Private ReadOnly _parameters As New List(Of DataFieldDefinitionWithValue)()

        Public Sub SetDataFieldValue(name As String, value As Object)
            Dim datafield = GetDataField(name)
            datafield.Value = value
        End Sub

        Public Function GetDataFieldValue(name As String) As Object
            Return GetDataField(name).Value
        End Function

        Private Function GetDataField(name As String) As DataFieldDefinitionWithValue
            Dim datafield = _parameters.SingleOrDefault(Function(df) df.Name = name)
            If datafield Is Nothing Then
                Throw New Exception("Data fields is not found")
            End If

            Return datafield
        End Function

        Friend Sub AddParameters(parameters As IEnumerable(Of DataFieldDefinitionWithValue))
            _parameters.RemoveAll(Function(ep) parameters.Count(Function(p) p.Name = ep.Name) > 0)
            _parameters.AddRange(parameters)
        End Sub
    End Class
End Namespace
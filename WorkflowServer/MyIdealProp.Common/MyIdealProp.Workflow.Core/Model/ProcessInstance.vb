Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Text
Imports System
Imports System.Runtime.Serialization

Namespace MyIdealProp.Workflow.Core.Model
    <Serializable()>
    <DataContract()> _
    Public Class ProcessInstance
        <DataMember>
        Public Property Id As Integer
        Public Property ProcessId() As Integer
        Public Property SchemeId() As Guid
        Friend Property ProcessScheme() As ProcessDefinition
        Public Property Status As ProcessStatus
        <DataMember>
        Public Property DisplayName As String
        <DataMember>
        Public Property Priority As ProcessPriority = ProcessPriority.Normal
        <DataMember>
        Public Property StartDate As DateTime
        <DataMember>
        Public Property Description As String

        <DataMember>
        Public Property Originator As String

        Private _procName As String
        <DataMember>
        Public Property ProcessName As String
            Get
                If String.IsNullOrEmpty(_procName) AndAlso ProcessScheme IsNot Nothing Then
                    _procName = ProcessScheme.Name
                End If

                Return _procName
            End Get
            Friend Set(value As String)
                _procName = value
            End Set
        End Property

        Public Property Settings As Dictionary(Of String, String)

        Public ReadOnly Property EndDate As DateTime?
            Get
                If ActivityInstanceLog.Count > 0 Then
                    Return ActivityInstanceLog.Last.EndDate
                End If

                Return Nothing
            End Get
        End Property

        Public Property ActivityInstanceLog As List(Of ActivityInstance)


        Public Property IsSchemeObsolete() As Boolean
            Get
                Return m_IsSchemeObsolete
            End Get
            Friend Set(value As Boolean)
                m_IsSchemeObsolete = value
            End Set
        End Property
        Private m_IsSchemeObsolete As Boolean

        Public Property IsDeterminingParametersChanged() As Boolean
            Get
                Return m_IsDeterminingParametersChanged
            End Get
            Friend Set(value As Boolean)
                m_IsDeterminingParametersChanged = value
            End Set
        End Property
        Private m_IsDeterminingParametersChanged As Boolean


        <DataMember>
        Public Property DataFields() As IEnumerable(Of DataFieldDefinitionWithValue)
            Get
                Return _processParameters
            End Get
            Friend Set(value As IEnumerable(Of DataFieldDefinitionWithValue))
                _processParameters.Clear()
                _processParameters.AddRange(value)
            End Set
        End Property

        Public Sub SetDataFieldValue(name As String, value As Object)
            Dim datafield = GetDataField(name)
            datafield.Value = value
        End Sub

        Public Function GetDataFieldValue(name As String) As Object
            Return GetDataField(name).Value
        End Function

        Private ReadOnly _processParameters As New List(Of DataFieldDefinitionWithValue)()

        Private Function GetDataField(name As String) As DataFieldDefinitionWithValue
            Dim datafield = _processParameters.SingleOrDefault(Function(df) df.Name = name)
            If datafield Is Nothing Then
                Throw New Exception("Data fields is not found")
            End If

            Return datafield
        End Function

        Friend Shared Function Create(processId As Integer, processScheme As ProcessDefinition, isSchemeObsolete As Boolean, isDeterminingParametersChanged As Boolean) As ProcessInstance
            Dim procInst = New ProcessInstance() With { _
                .ProcessId = processId, _
                .ProcessScheme = processScheme, _
                .IsSchemeObsolete = isSchemeObsolete, _
                .IsDeterminingParametersChanged = isDeterminingParametersChanged,
                .Originator = "",
                .StartDate = DateTime.Now,
                .ActivityInstanceLog = New List(Of ActivityInstance)
            }
            procInst.AddParameters(processScheme.DataFields.Select(Function(df) DataFieldDefinitionWithValue.CreateWithDefaultValue(df)))

            Return procInst
        End Function

        Friend Sub UpdateProcessScheme(procScheme As ProcessDefinition)
            Me.ProcessScheme = procScheme
        End Sub

        Friend Function GetClientInstance(actInstId As Integer) As EventInstance
            Return ActivityInstanceLog.Single(Function(act) act.ActInstId = actInstId).ClientEventInst
        End Function

        Friend Function GetClientInstance(actInstId As Integer, destUser As String) As EventInstance
            Dim actInst = ActivityInstanceLog.Single(Function(act) act.ActInstId = actInstId)
            If actInst.ActivityDestinationInstances IsNot Nothing AndAlso actInst.ActivityDestinationInstances.Count > 0 Then
                Dim actDestInst = actInst.ActivityDestinationInstances.Single(Function(adInst) adInst.DestinationUser.ToLower = destUser.ToLower)
                Return actDestInst.ClientEventInst
            Else
                Return ActivityInstanceLog.Single(Function(act) act.ActInstId = actInstId).ClientEventInst
            End If
        End Function

        Friend Sub AddParameter(parameter As DataFieldDefinitionWithValue)
            _processParameters.RemoveAll(Function(p) p.Name = parameter.Name)
            _processParameters.Add(parameter)
        End Sub

        Friend Sub AddParameters(parameters As IEnumerable(Of DataFieldDefinitionWithValue))
            _processParameters.RemoveAll(Function(ep) parameters.Count(Function(p) p.Name = ep.Name) > 0)
            _processParameters.AddRange(parameters)
        End Sub

        Public Function GetParameter(name As String) As DataFieldDefinitionWithValue
            Return _processParameters.SingleOrDefault(Function(p) p.Name = name)
        End Function

        'Friend nextActivityName As String
        'Public Sub GotoActivity(activityName As String)
        '    nextActivityName = activityName
        'End Sub

        Friend ReadOnly Property CurrentActivityName() As String
            Get
                Dim parameter = GetParameter(DefaultDefinitions.ParameterCurrentActivity.Name)
                Return If(parameter Is Nothing, Nothing, DirectCast(parameter.Value, String))
            End Get
        End Property

        Friend ReadOnly Property CurrentActivity() As ActivityDefinition
            Get
                Return ProcessScheme.FindActivity(CurrentActivityName)
            End Get
        End Property

        Friend Sub SetProcessParameters(pd As List(Of DataFieldDefinitionWithValue))
            _processParameters.Clear()
            AddParameters(pd)
        End Sub

        Friend Sub AddActivityInstance(actInst As ActivityInstance)
            ActivityInstanceLog.Add(actInst)
        End Sub

        Enum ProcessPriority
            Normal = 0
            Important = 1
            Urgent = 2
        End Enum
    End Class


End Namespace

Imports System.ServiceModel
Imports System.ServiceModel.Security

Public Class Connection
    Implements IDisposable

    Public Property WorkflowServer As String
    Public Property UserName As String
    Public Property Password As String
    Public Property Integrated As Boolean = True

    Private wfServer As WorkflowServer.WorkflowServiceClient

    Public Sub New(wfServer As String)
        Me.WorkflowServer = wfServer
    End Sub

#Region "Open Connection"
    Public Sub Open()
        Dim binding As New NetTcpBinding
        binding.Security.Mode = SecurityMode.Message
        binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName
        binding.MaxReceivedMessageSize = 2147483647
        binding.MaxBufferSize = 2147483647

        'binding.ReaderQuotas.MaxStringContentLength = 8192
        'binding.ReaderQuotas.MaxDepth = 32
        'binding.ReaderQuotas.MaxArrayLength = 16384
        'binding.ReaderQuotas.MaxBytesPerRead = 4096
        'binding.ReaderQuotas.MaxNameTableCharCount = 16384

        Dim endpointId = EndpointIdentity.CreateDnsIdentity("WorkflowService")
        Dim myEndPoint As New EndpointAddress(New Uri(String.Format("net.tcp://{0}:8000/WorkflowService", WorkflowServer)), endpointId)
        wfServer = New WorkflowServer.WorkflowServiceClient(binding, myEndPoint)
        wfServer.ClientCredentials.UserName.UserName = UserName
        If Integrated Then
            wfServer.ClientCredentials.UserName.Password = "IntegratedPasswrod"
        Else
            wfServer.ClientCredentials.UserName.Password = Password
        End If

        wfServer.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None
        wfServer.Open()
    End Sub
#End Region

#Region "Process Instance"

    Public Function CreateProcessInstance(procName As String) As ProcessInstance
        Return ProcessInstance.Create(procName)
    End Function

    Public Function StartProcessInstance(procInst As ProcessInstance) As Integer
        Try
            Dim pInst = wfServer.StartProcessInstance(procInst.ProcessName, procInst.DisplayName, CType(procInst.Priority, WorkflowServer.ProcessInstance.ProcessPriority), procInst.DataFields)
            Return pInst.Id
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetProcessInstancesByDataFields(procName As String, key As String, value As String) As List(Of Integer)
        Return wfServer.GetProcessInstancesByDataFields(procName, key, value).ToList
    End Function

    Public Sub ExpiredProcessInstance(procInstId As Integer)
        wfServer.ExpiredProcessInstance(procInstId)
    End Sub
#End Region

#Region "Worklist"
    Public Function OpenMyWorklist() As List(Of WorklistItem)
        Dim items = wfServer.GetWorkList()
        Dim wl As New List(Of WorklistItem)
        For Each item In items
            wl.Add(WorklistItem.Create(item))
        Next
        Return wl
    End Function

    Public Function OpenWorklistItem(seriesNumber As String) As WorklistItem
        Dim procInstId = seriesNumber.Split("_")(0)
        Dim actInstId = seriesNumber.Split("_")(1)

        Dim item = WorklistItem.Create(wfServer.OpenWorkListItem(procInstId, actInstId))
        item.Connection = Me
        Return item
    End Function

    Public Sub CompleteWorklistItem(wli As WorklistItem)
        Try
            If wli.ActivityDestinationInstance IsNot Nothing Then
                wfServer.CompleteDestInstWorklist(wli.ProcInstId, wli.ActInstId, wli.ProcessInstance.DataFields, wli.ActivityDestinationInstance.DataFields)
            Else
                wfServer.CompleteWorkList(wli.ProcInstId, wli.ActInstId, wli.ProcessInstance.DataFields)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Friend Sub GotoActivity(procInstId As Integer, actName As String)
        Try
            wfServer.GotoActivity(procInstId, actName)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub Close()
        wfServer.Close()
    End Sub
#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
            wfServer.Close()
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
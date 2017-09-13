Imports System.ServiceModel
Imports MyIdealProp.Workflow.Core.Model
Imports MyIdealProp.Workflow.Core.Runtime
Imports MyIdealProp.Common

Namespace MyIdealProp.Workflow.Service
    Public Class WorkflowService
        Implements IWorkflowService


        Dim runTime As WorkflowRuntime = WorkflowRuntime.GetInstance

#Region "Process Instance update"
        Public Function StartProcessInstance(procName As String, DisplayName As String, priority As ProcessInstance.ProcessPriority, dataFields As Dictionary(Of String, Object)) As ProcessInstance Implements IWorkflowService.StartProcessInstance

            Try
                Dim procInst As ProcessInstance = runTime.CreateProcessInstance(procName)
                procInst.Originator = CurrentUser()
                procInst.Priority = priority
                procInst.DisplayName = DisplayName
                procInst.StartDate = DateTime.Now

                For Each df In dataFields
                    procInst.SetDataFieldValue(df.Key, df.Value)
                Next

                runTime.StartProcessInstance(procInst)
                Return procInst
            Catch ex As Exception
                Logger.Log.Error("Client Sevice Error: Start Process Instance ", ex)
                Return Nothing
            End Try
        End Function

        Public Function CreateProcessInstance(procName As String) As ProcessInstance Implements IWorkflowService.CreateProcessInstance
            Try
                Dim procInst As ProcessInstance = runTime.CreateProcessInstance(procName)
                Return procInst
            Catch ex As Exception
                Logger.Log.Error("Client Sevice Error: Start Process Instance ", ex)
                Return Nothing
            End Try
        End Function

        Public Sub ExpiredProcessInstance(procInstId As Integer) Implements IWorkflowService.ExpiredProcessInstance
            Try
                runTime.TeminateProcessInstance(procInstId)
            Catch ex As Exception
                Logger.Log.Error("Client Service Erroe: Expired Process Instance " & procInstId, ex)
            End Try
        End Sub

        Public Function GetProcessInstancesByDataFields(procName As String, key As String, value As String) As List(Of Integer) Implements IWorkflowService.GetProcessInstancesByDataFields
            Try
                Return runTime.GetProcInstsByDataFields(procName, key, value)
            Catch ex As Exception
                Logger.Log.Error(String.Format("Client Service Error: GetProcessInstanceByDatafields,  ProcName:{0}, Key:{1}, Value:{2}", procName, key, value), ex)
                Return New List(Of Integer)
            End Try
        End Function
#End Region

#Region "Worklist Item"

        Public Function GetWorkList() As List(Of WorklistItem) Implements IWorkflowService.GetWorkList
            Try
                Return runTime.GetWorklist(CurrentUser)
            Catch ex As Exception
                Logger.Log.Error(String.Format("Client Service Error: GetWorkList,  UserName:{0}", CurrentUser), ex)
                Return New List(Of WorklistItem)
            End Try
        End Function

        Public Function OpenWorkListItem(procInstId As Integer, actInstId As Integer) As WorklistItem Implements IWorkflowService.OpenWorkListItem
            Return runTime.OpenWorklistItem(procInstId, actInstId, CurrentUser)
        End Function

        Public Function UpdateWorklistItem(procInstId As Integer, actInstId As Integer, dataFields As Dictionary(Of String, Object)) As Object Implements IWorkflowService.UpdateWorklistItem
            Throw New NotImplementedException
        End Function

        Public Sub CompleteWorkList(procInstId As Integer, actInstId As Integer, datafields As Dictionary(Of String, Object)) Implements IWorkflowService.CompleteWorkList
            Try
                runTime.CompleteWorklist(procInstId, actInstId, CurrentUser, datafields)
            Catch ex As Exception
                Logger.Log.Error("Client Service Error: CompleteWorklist " & procInstId, ex)
            End Try
        End Sub

        Public Sub CompleteDestInstWorklist(procInstId As Integer, actInstId As Integer, datafields As Dictionary(Of String, Object), actDataFields As Dictionary(Of String, Object)) Implements IWorkflowService.CompleteDestInstWorklist
            Try
                runTime.CompleteWorklist(procInstId, actInstId, CurrentUser, datafields, actDataFields)
            Catch ex As Exception
                Logger.Log.Error("Client Service Error: CompleteWorklist " & procInstId, ex)
            End Try
        End Sub

        Public Sub GotoActivity(procInstId As Integer, actName As String) Implements IWorkflowService.GotoActivity
            Try
                runTime.GotoActivity(procInstId, actName)
            Catch ex As Exception
                Logger.Log.Error("Client Service Error: GotoActivity Error, ProcInstId: " & procInstId & " Activity Name: " & actName)
            End Try
        End Sub
#End Region

#Region "Private help methods"

        Private Function CurrentUser() As String
            Return ServiceSecurityContext.Current.PrimaryIdentity.Name
        End Function

        Private Function GetProcInstIdFromSN(seriesNumber As String) As Integer
            Return CInt(seriesNumber.Split("_")(0))
        End Function

        Private Function GetActInstIdFromSN(seriesNumber As String) As Integer
            Return CInt(seriesNumber.Split("_")(1))
        End Function

#End Region




    End Class
End Namespace
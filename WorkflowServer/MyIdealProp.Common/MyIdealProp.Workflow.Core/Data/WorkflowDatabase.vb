Imports System.Data.SqlClient
Imports MyIdealProp.Workflow.Core.Model
Imports MyIdealProp.Workflow.Core.Fault
Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Namespace MyIdealProp.Workflow.Core.Data
    Public Class WorkflowDatabase

        Private sqlHelper As SQLHelper
        Public Sub New()
            sqlHelper = New SQLHelper
        End Sub

#Region "Process Scheme"
        Public Function GetProcessSchemeByName(name As String) As SchemeDefinition(Of XElement)
            Dim result As SchemeDefinition(Of XElement)
            Dim params As New List(Of SqlParameter)
            params.Add(New SqlParameter("@Name", name))
            Dim reader = sqlHelper.ExecuteReaderSP("GetProcessSchemeByName", params.ToArray)
            If reader.Read Then
                result = New SchemeDefinition(Of XElement)(CInt(reader("ProcessId")), XElement.Parse(reader("Scheme")), False)
            Else
                Throw New SchemeNotFoundException()
            End If
            reader.Close()
            Return result
        End Function

        Public Function GetProcessSchemeById(processId As Integer) As SchemeDefinition(Of XElement)
            Dim result As SchemeDefinition(Of XElement)
            Dim params As New List(Of SqlParameter)
            params.Add(New SqlParameter("@ProcessId", processId))
            Dim reader = sqlHelper.ExecuteReaderSP("GetProcessSchemeById", params.ToArray)
            If reader.Read Then
                result = New SchemeDefinition(Of XElement)(CInt(reader("ProcessId")), XElement.Parse(reader("Scheme")), False)
            Else
                Throw New SchemeNotFoundException()
            End If
            reader.Close()
            Return result
        End Function

        Public Function GetAllProcess() As List(Of SchemeDefinition(Of XElement))
            Dim results As New List(Of SchemeDefinition(Of XElement))
            Dim reader = sqlHelper.ExecuteReaderSP("GetAllProcessScheme")
            While reader.Read
                results.Add(New SchemeDefinition(Of XElement)(CInt(reader("ProcessId")), XElement.Parse(reader("Scheme")), False))
            End While
            reader.Close()
            Return results
        End Function

        Public Function SaveScheme(processName As String, displayName As String, description As String, scheme As String) As Integer
            Dim params As New List(Of SqlParameter)
            params.AddRange({New SqlParameter("@Name", processName),
                             New SqlParameter("@DisplayName", displayName),
                             New SqlParameter("@Description", description),
                             New SqlParameter("@Scheme", scheme)})
            Return CInt(sqlHelper.ExecuteScalarSP("SaveProcessScheme", params.ToArray))
        End Function
#End Region

#Region "Process Instance"
        Public Sub InitialProcessInstance(procInst As ProcessInstance)
            Dim params As New List(Of SqlParameter)

            Dim tvp = New DataTable
            tvp.Columns.Add("Name")
            tvp.Columns.Add("Value")

            For Each df In procInst.DataFields
                Dim row = tvp.NewRow
                row(0) = df.Name
                row(1) = df.Value

                tvp.Rows.Add(row)
            Next

            params.AddRange(From p In {New SqlParameter("@ProcessId", procInst.ProcessId),
                                       New SqlParameter("@DisplayName", procInst.DisplayName),
                                       New SqlParameter("@Priority", procInst.Priority),
                                       New SqlParameter("@Originator", procInst.Originator),
                                       New SqlParameter("@Description", procInst.Description),
                                       New SqlParameter("@StartDate", procInst.StartDate),
                                       New SqlParameter("@Status", procInst.Status.Id),
                                       New SqlParameter("@InstData", SerializedProcess(procInst)),
                                       New SqlParameter("@Fields", SqlDbType.Structured)
                    })
            params.Last.Value = tvp
            procInst.Id = sqlHelper.ExecuteScalarSP("CreateProcessInstance", params.ToArray)
        End Sub

        Public Sub SaveProcInstDataFields(procInst As ProcessInstance)
            Dim params As New List(Of SqlParameter)

            Dim tvp = New DataTable
            tvp.Columns.Add("Name")
            tvp.Columns.Add("Value")

            For Each df In procInst.DataFields
                Dim row = tvp.NewRow
                row(0) = df.Name
                row(1) = df.Value

                tvp.Rows.Add(row)
            Next

            params.AddRange(From p In {New SqlParameter("@ProcessId", procInst.ProcessId),
                                       New SqlParameter("@Fields", SqlDbType.Structured)
                    })
            params.Last.Value = tvp
            procInst.Id = sqlHelper.ExecuteScalarSP("SaveProcInstDataFields", params.ToArray)
        End Sub

        Public Sub SaveProcessStatus(procInst As ProcessInstance)
            Dim params As New List(Of SqlParameter)
            Dim tvp = New DataTable
            tvp.Columns.Add("Name")
            tvp.Columns.Add("Value")

            For Each df In procInst.DataFields
                Dim row = tvp.NewRow
                row(0) = df.Name
                row(1) = df.Value

                tvp.Rows.Add(row)
            Next

            params.AddRange({New SqlParameter("@ProcInstId", procInst.Id),
                             New SqlParameter("@Status", procInst.Status.Id),
                             New SqlParameter("@ProcInstData", SerializedProcess(procInst)),
                             New SqlParameter("@Fields", SqlDbType.Structured)
                    })
            params.Last.Value = tvp
            sqlHelper.ExecuteNonQuerySP("SaveProcInstStatus", params.ToArray)
        End Sub

        Public Function GetRunningProcInst(procInstId As Integer) As ProcessInstance
            Dim params As New List(Of SqlParameter)
            params.Add(New SqlParameter("@ProcInstId", procInstId))
            Dim data = sqlHelper.ExecuteScalarSP("GetRunningProcInstData", params.ToArray)
            If data IsNot Nothing Then
                Return DeserializeProcess(data)
            Else
                Throw New Exception("Process Instance is not existed.")
            End If
        End Function

        Public Function GetProcInstToExecutor() As List(Of ProcessInstance)
            Dim reader = sqlHelper.ExecuteReaderSP("GetRunningProcInsts")
            Dim procInsts As New List(Of ProcessInstance)

            While reader.Read
                If Not reader.IsDBNull(0) Then
                    procInsts.Add(DeserializeProcess(reader(0)))
                End If
            End While
            reader.Close()
            Return procInsts
        End Function

        Public Function GetProcInstsByDataFields(procName As String, key As String, value As String) As List(Of Integer)
            Dim params As New List(Of SqlParameter)
            params.AddRange({New SqlParameter("@ProcName", procName),
                             New SqlParameter("@Key", key),
                             New SqlParameter("@Value", value)})
            Dim reader = sqlHelper.ExecuteReaderSP("GetProcessInstanceByDataFields", params.ToArray)

            Dim procInsts As New List(Of Integer)
            While reader.Read
                If Not reader.IsDBNull(0) Then
                    procInsts.Add(reader.GetInt32(0))
                End If
            End While
            reader.Close()
            Return procInsts
        End Function

        Private Function DeserializeProcess(instData() As Byte) As ProcessInstance
            Dim serializer As New BinaryFormatter
            Dim writer As New MemoryStream(instData)
            Dim procInst As Model.ProcessInstance = serializer.Deserialize(writer)
            Return procInst
        End Function

        Private Function SerializedProcess(procInst As Model.ProcessInstance) As Byte()
            Using writer As New MemoryStream()
                Dim serializer As New BinaryFormatter
                serializer.Serialize(writer, procInst)
                writer.Flush()

                Return writer.ToArray
            End Using
        End Function
#End Region

#Region "Activity Instance"
        Public Sub CreateActivityInstance(actInst As ActivityInstance)
            Dim params As New List(Of SqlParameter)

            Dim tvp = New DataTable
            tvp.Columns.Add("DueTime")
            tvp.Columns.Add("ActionName")
            tvp.Columns.Add("RepeatNo")
            tvp.Columns.Add("CreateTime")
            tvp.Columns.Add("Status")

            For Each escalInst In actInst.EscalationInstances
                Dim row = tvp.NewRow
                row(0) = escalInst.ExecuteTime
                row(1) = escalInst.ActionName
                row(2) = escalInst.RepeatNo
                row(3) = DateTime.Now
                row(4) = CInt(EscalationInstance.EscalationStatus.Active)

                tvp.Rows.Add(row)
            Next

            params.AddRange({New SqlParameter("@ProcInstId", actInst.ProcessInstance.Id),
                             New SqlParameter("@Name", actInst.ActivityName),
                             New SqlParameter("@DestUser", String.Join(";", actInst.DestinationUsers.ToArray)),
                             New SqlParameter("@StartDate", actInst.StartDate),
                             New SqlParameter("@Status", CInt(actInst.Status)),
                             New SqlParameter("@EscalInsts", SqlDbType.Structured)
                    })
            params.Last.Value = tvp

            actInst.ActInstId = CInt(sqlHelper.ExecuteScalarSP("CreateActivityInstance", params.ToArray))
        End Sub

        Public Sub UpdateActivityInstance(actInst As ActivityInstance)
            Dim params As New List(Of SqlParameter)
            params.AddRange({New SqlParameter("@ActInstId", actInst.ActInstId),
                             New SqlParameter("@Status", actInst.Status)
                            })
            sqlHelper.ExecuteScalarSP("UpdateActivityInstance", params.ToArray)
        End Sub

#End Region

#Region "Activity Destination Instance"

        Public Sub CreateActDestInst(actDestInst As ActivityDestinationInstance)
            Dim params As New List(Of SqlParameter)

            Dim tvp = New DataTable
            tvp.Columns.Add("Name")
            tvp.Columns.Add("Value")

            For Each df In actDestInst.DataFields
                Dim row = tvp.NewRow
                row(0) = df.Name
                row(1) = df.Value

                tvp.Rows.Add(row)
            Next

            params.AddRange({New SqlParameter("@ProcInstId", actDestInst.ProcessInstance.Id),
                             New SqlParameter("@ActInstId", actDestInst.ActivityInstance.ActInstId),
                             New SqlParameter("@DestUser", actDestInst.DestinationUser),
                             New SqlParameter("@StartDate", actDestInst.StartTime),
                             New SqlParameter("@Status", CInt(actDestInst.Status)),
                             New SqlParameter("@DataFields", SqlDbType.Structured)
                    })
            params.Last.Value = tvp

            actDestInst.Id = CInt(sqlHelper.ExecuteScalarSP("CreateActivityDestinationInstance", params.ToArray))
        End Sub

        Public Sub UpdateActivityDestinationInstance(actDestInst As ActivityDestinationInstance)
            Dim params As New List(Of SqlParameter)
            params.AddRange({New SqlParameter("@ActDestInstId", actDestInst.Id),
                             New SqlParameter("@Status", actDestInst.Status)
                            })
            sqlHelper.ExecuteScalarSP("UpdateActivityDestinationInstance", params.ToArray)
        End Sub

        Public Sub SaveActivityDataFields(actDestInst As ActivityDestinationInstance)
            Dim params As New List(Of SqlParameter)

            Dim tvp = New DataTable
            tvp.Columns.Add("Name")
            tvp.Columns.Add("Value")

            For Each df In actDestInst.DataFields
                Dim row = tvp.NewRow
                row(0) = df.Name
                row(1) = df.Value

                tvp.Rows.Add(row)
            Next

            params.AddRange({New SqlParameter("@ActDestInstId", actDestInst.Id),
                             New SqlParameter("@DataFields", SqlDbType.Structured)
                    })
            params.Last.Value = tvp

            sqlHelper.ExecuteScalarSP("SaveActivityDataFields", params.ToArray)
        End Sub
#End Region

#Region "Line Instance"
        Public Sub CreateLineInstance(lineInst As LineInstance)
            Dim params As New List(Of SqlParameter)
            params.AddRange({New SqlParameter("@ProcInstId", lineInst.ProcessInstance.Id),
                             New SqlParameter("@From", lineInst.LineDefinition.From.Name),
                             New SqlParameter("@To", lineInst.LineDefinition.To.Name),
                             New SqlParameter("@RuleResult", lineInst.LineRuleResult),
                             New SqlParameter("@CreateDate", DateTime.Now)
                    })

            lineInst.LineInstId = CInt(sqlHelper.ExecuteScalarSP("CreateLineInstance", params.ToArray))
        End Sub
#End Region

#Region "Event Instance"
        Public Sub CreateEventInstance(eventInst As EventInstance)
            Dim params As New List(Of SqlParameter)
            params.AddRange({
                            New SqlParameter("@ActInstId", eventInst.ActivityInstance.ActInstId),
                            New SqlParameter("@ProcInstId", eventInst.ActivityInstance.ProcessInstance.Id),
                            New SqlParameter("@ActDestInstId", If(eventInst.ActivityDestnationInstance IsNot Nothing, eventInst.ActivityDestnationInstance.Id, 0)),
                            New SqlParameter("@Name", eventInst.EventName),
                            New SqlParameter("@Type", eventInst.IsClient),
                            New SqlParameter("@StartDate", eventInst.StartDate),
                            New SqlParameter("@Status", eventInst.Status)
                            })
            eventInst.Id = CInt(sqlHelper.ExecuteScalarSP("CreateEventInstance", params.ToArray))
        End Sub

        Public Sub CompleteEvent(eventInst As EventInstance)
            Dim params As New List(Of SqlParameter)
            params.AddRange({New SqlParameter("@EventInstId", eventInst.Id),
                             New SqlParameter("@EndDate", eventInst.EndDate),
                             New SqlParameter("@Status", eventInst.Status)
                            })
            sqlHelper.ExecuteScalarSP("CompleteEventIntance", params.ToArray)
        End Sub
#End Region

#Region "Worklist"
        Public Sub AddWorklistItem(wli As WorklistItem)
            Dim params As New List(Of SqlParameter)
            Dim eventInst = wli.EventInstance
            params.AddRange({New SqlParameter("@ProcInstId", eventInst.ProcessInstance.Id),
                             New SqlParameter("@ActInstId", eventInst.ActivityInstance.ActInstId),
                             New SqlParameter("@EventInstId", eventInst.Id),
                             New SqlParameter("@ItemData", wli.ItemData),
                             New SqlParameter("@ActivityName", eventInst.ActivityName),
                             New SqlParameter("@DestinationUser", wli.Destination),
                             New SqlParameter("@CreateDate", eventInst.StartDate),
                             New SqlParameter("@Status", wli.Status)
                            })
            sqlHelper.ExecuteScalarSP("CreateWorklistItem", params.ToArray)
        End Sub

        Public Function GetMyWorklist(destUser As String) As List(Of WorklistItem)
            Dim params As New List(Of SqlParameter)
            params.AddRange({New SqlParameter("@DestinationUser", destUser)
                            })
            Dim reader = sqlHelper.ExecuteReaderSP("GetMyWorklist", params.ToArray)
            Dim wl As New List(Of WorklistItem)
            While reader.Read
                wl.Add(WorklistItem.Create(reader.GetInt32(1),
                                           reader.GetInt32(2),
                                           reader.GetInt32(3),
                                           If(Not reader.IsDBNull(4), reader.GetString(4), Nothing),
                                           reader.GetString(5),
                                            reader.GetString(6),
                                           If(Not reader.IsDBNull(7), reader.GetDateTime(7), Nothing),
                                            reader.GetInt32(8),
                                            If(Not reader.IsDBNull(9), reader.GetString(9), ""),
                                            If(Not reader.IsDBNull(10), reader.GetString(10), ""),
                                            If(Not reader.IsDBNull(11), reader.GetString(11), ""),
                                            If(Not reader.IsDBNull(12), reader.GetInt32(12), 0),
                                            If(Not reader.IsDBNull(13), reader.GetString(13), "")
                                           ))
            End While
            reader.Close()
            Return wl
        End Function

        Public Function OpenWorklistItem(procInstId As Integer, actInstId As Integer, destUser As String) As WorklistItem
            Dim params As New List(Of SqlParameter)
            params.AddRange({New SqlParameter("@ProcInstId", procInstId),
                             New SqlParameter("@ActInstId", actInstId),
                             New SqlParameter("@DestUser", destUser)
                            })
            Dim reader = sqlHelper.ExecuteReaderSP("OpenWorklistItem", params.ToArray)
            Dim wli As WorklistItem
            If reader.Read Then
                wli = WorklistItem.Create(reader.GetInt32(1),
                                           reader.GetInt32(2),
                                           reader.GetInt32(3),
                                           If(Not reader.IsDBNull(4), reader.GetString(4), Nothing),
                                           reader.GetString(5),
                                            reader.GetString(6),
                                           If(Not reader.IsDBNull(7), reader.GetDateTime(7), Nothing),
                                            reader.GetInt32(8),
                                            If(Not reader.IsDBNull(9), reader.GetString(9), ""),
                                            If(Not reader.IsDBNull(10), reader.GetString(10), ""),
                                            If(Not reader.IsDBNull(11), reader.GetString(11), ""),
                                            If(Not reader.IsDBNull(13), reader.GetInt32(13), 0),
                                            If(Not reader.IsDBNull(14), reader.GetString(14), "")
                                           )
                Dim procInst = DeserializeProcess(CType(reader.GetValue(12), Byte()))

                'change to actdestinst
                'wli.EventInstance = procInst.GetClientInstance(actInstId)
                wli.EventInstance = procInst.GetClientInstance(actInstId, destUser)

                reader.Close()
                Return wli
            Else
                reader.Close()
                Throw New Exception("work list is not exsited.")
            End If
        End Function

        Public Sub CompleteWorklistItem(wli As WorklistItem)
            Dim params As New List(Of SqlParameter)
            params.AddRange({New SqlParameter("@ProcInstId", wli.ProcInstId),
                             New SqlParameter("@ActInstId", wli.ActInstId),
                             New SqlParameter("@DestUser", wli.Destination),
                             New SqlParameter("@EndDate", DateTime.Now)
                            })
            sqlHelper.ExecuteNonQuerySP("CompleteWorklistItem", params.ToArray)
        End Sub

        Public Sub ExpiredWorklistItem(procInstId As Integer, actInstId As Integer)
            Dim params As New List(Of SqlParameter)
            params.AddRange({New SqlParameter("@ProcInstId", procInstId),
                             New SqlParameter("@ActInstId", actInstId)
                            })
            sqlHelper.ExecuteNonQuerySP("ExpiredWorklistItem", params.ToArray)
        End Sub
#End Region

#Region "Escalation Timer"
        Public Function GetNextTimer() As TimerInstance

            Dim reader = sqlHelper.ExecuteReaderSP("GetNextTimer")
            If reader.Read Then
                Dim timer = New TimerInstance With
                       {
                           .TimerId = reader.GetInt32(0),
                           .EscalationId = reader.GetInt32(1),
                           .ProcInstId = reader.GetInt32(2),
                           .ActInstId = reader.GetInt32(3),
                           .DueTime = reader.GetDateTime(4),
                           .ActionName = reader.GetString(5)
                           }
                reader.Close()
                Return timer
            Else
                reader.Close()
                Return Nothing
            End If
        End Function

        Public Sub FinishTimer(timerId As Integer)
            Dim params As New List(Of SqlParameter)
            params.AddRange({New SqlParameter("TimerId", timerId),
                             New SqlParameter("ExecuteTime", DateTime.Now)
                            })

            sqlHelper.ExecuteNonQuerySP("FinishTimer", params.ToArray)
        End Sub
#End Region

#Region "Error Logs"
        Public Sub AddErrorLogs(log As ErrorLog)
            Dim params As New List(Of SqlParameter)
            params.AddRange({New SqlParameter("@ProcInstId", log.ProcInstId),
                             New SqlParameter("@ActInstid", log.ActInstId),
                             New SqlParameter("@EventInstId", log.EventInstId),
                             New SqlParameter("@Source", log.Source),
                             New SqlParameter("@ErrorMsg", log.ErrorMsg)
                            })
            sqlHelper.ExecuteNonQuerySP("AddErrorlog", params.ToArray)
        End Sub

        Public Function GetErrorLog(logId As Integer) As ErrorLog
            Dim params As New List(Of SqlParameter)
            params.Add(New SqlParameter("@LogId", logId))

            Dim reader = sqlHelper.ExecuteReaderSP("GetErrorLog", params.ToArray)
            If reader.Read Then
                Dim log = BuildErrorlog(reader)
                reader.Close()
                Return log
            Else
                reader.Close()
                Throw New Exception(String.Format("Error {0} is not found.", logId))
            End If
        End Function

        Public Function GetAllErrorLogs() As List(Of ErrorLog)
            Dim logs As New List(Of ErrorLog)
            Dim reader = sqlHelper.ExecuteReaderSP("GetAllErrorLogs")
            While reader.Read
                logs.Add(BuildErrorlog(reader))
            End While
            reader.Close()
            Return logs
        End Function

        Public Sub RetryError(logId As Integer)
            Dim params As New List(Of SqlParameter)
            params.Add(New SqlParameter("@LogId", logId))
            sqlHelper.ExecuteNonQuerySP("DeleteErrorlog", params.ToArray)
        End Sub

        Private Function BuildErrorlog(reader As SqlDataReader) As ErrorLog
            Dim errorLog As New ErrorLog
            errorLog.LogId = CInt(reader(0))
            errorLog.ProcInstId = reader.GetInt32(1)
            errorLog.ActInstId = reader.GetInt32(2)
            errorLog.EventInstId = reader.GetInt32(3)
            errorLog.Source = reader.GetString(4)
            errorLog.ErrorMsg = If(Not reader.IsDBNull(5), reader.GetString(5), "")
            errorLog.CreateTime = reader.GetDateTime(6)

            Return errorLog
        End Function
#End Region

#Region "Workflow Server"

        Public Sub UpdateLastRunningTime(serverName As String)
            Dim params As New List(Of SqlParameter)
            params.AddRange({New SqlParameter("@ServerName", serverName)})
            sqlHelper.ExecuteNonQuerySP("UpdateServerRunningTime", params.ToArray)
        End Sub
#End Region

#Region "Process Settings"
        Public Function GetSettingsValue(procName As String, name As String) As String
            Dim params As New List(Of SqlParameter)
            params.AddRange({New SqlParameter("@ProcessName", procName),
                             New SqlParameter("@Name", name)
                            })
            Dim result = sqlHelper.ExecuteScalarSP("GetProcessSettingValue", params.ToArray)
            If result IsNot Nothing Then
                Return result.ToString
            Else
                Return ""
            End If
        End Function
#End Region

    End Class
End Namespace

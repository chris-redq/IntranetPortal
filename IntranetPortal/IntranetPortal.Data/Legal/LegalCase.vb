﻿Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json.Linq

''' <summary>
''' The legal data case object
''' </summary>
Partial Public Class LegalCase

    Public Const ForeclosureStatusCategory As String = "LegalFCDataStatus"
    Public Const SecondaryTypeStatusCategory As String = "LegalSecondaryType"

    Public Const TitleSaveLog As String = "LegalSave"

    Private _stuatsStr As String
    Public ReadOnly Property StuatsStr As String
        Get
            If _stuatsStr Is Nothing Then
                If (Status.HasValue) Then
                    _stuatsStr = CType(Status, LegalCaseStatus).ToString
                End If

            End If
            Return _stuatsStr
        End Get
    End Property

    Private _caseStatus As String
    Public ReadOnly Property CaseStatus As String
        Get
            If _caseStatus Is Nothing Then
                Dim mCaseDate = Newtonsoft.Json.Linq.JObject.Parse(CaseData)
                _caseStatus = mCaseDate.Item("CaseStauts")
            End If
            Return _caseStatus
        End Get
    End Property

    'Private _saleDate As DateTime
    'Public ReadOnly Property SaleDate As Date
    '    Get
    '        If (_saleDate Is Nothing) Then

    '        End If
    '        Return _saleDate
    '    End Get
    'End Property

    Public ReadOnly Property LegalStatusString As String
        Get
            If LegalStatus.HasValue Then
                Dim ls As DataStatu = DataStatu.Instance(ForeclosureStatusCategory, LegalStatus)
                Return ls.Name
            End If

            Return Nothing
        End Get
    End Property

    Public Sub SaveData(saveBy As String)
        'Refresh data report fields
        RefreshReportFields()

        Using ctx As New PortalEntities
            If Not ctx.LegalCases.Any(Function(l) l.BBLE = BBLE) Then
                Me.CreateDate = DateTime.Now
                Me.CreateBy = saveBy
                ctx.LegalCases.Add(Me)
            Else
                Me.UpdateBy = saveBy
                Me.UpdateDate = DateTime.Now
                ctx.Entry(Me).State = Entity.EntityState.Modified
            End If

            ctx.SaveChanges()
        End Using

        Core.SystemLog.Log(TitleSaveLog, Newtonsoft.Json.JsonConvert.SerializeObject(Me), Core.SystemLog.LogCategory.SaveData, Me.BBLE, saveBy)
    End Sub

    Private Sub RefreshReportFields()
        Dim jsonCase = Newtonsoft.Json.Linq.JObject.Parse(CaseData)

        If jsonCase IsNot Nothing Then
            Dim caseStatus = jsonCase.Item("CaseStauts")
            Dim result As Integer
            If Integer.TryParse(caseStatus, result) Then
                If result > 0 Then
                    Me.LegalStatus = result
                End If
            End If

            Dim sTypes = jsonCase.Item("SecondaryTypes")

            If (sTypes IsNot Nothing) Then
                Me.SecondaryTypes = sTypes.ToString
            End If
            Dim forecloseInfo = jsonCase.Item("ForeclosureInfo")
            If (forecloseInfo IsNot Nothing AndAlso forecloseInfo.Item("FCIndexNum") IsNot Nothing) Then
                Me.FCIndexNum = forecloseInfo.Item("FCIndexNum").ToString().Trim()
            End If

            Dim vTaxLienIndexNum = jsonCase.Item("TaxLienIndexNum")
            If (vTaxLienIndexNum IsNot Nothing) Then
                Me.TaxLienIndexNum = vTaxLienIndexNum
            End If

            Dim vTaxLienFCStatus = jsonCase.Item("TaxLienFCStatus")
            If (vTaxLienFCStatus IsNot Nothing) Then
                Me.TaxLienFCStatus = vTaxLienFCStatus
            End If
            Dim data = jsonCase.Item("SaleDate")

            If String.IsNullOrEmpty(data) Then
                Me.SaleDate = Nothing
            Else
                Dim saleData As DateTime
                If data IsNot Nothing AndAlso DateTime.TryParse(data.ToString, saleData) Then
                    If saleData > DateTime.MinValue Then
                        Me.SaleDate = saleData
                    Else
                        Me.SaleDate = Nothing
                    End If
                End If
            End If
        End If
    End Sub

    Private _caseJsonObject As JObject

    ''' <summary>
    ''' Return legal case data in Json object
    ''' </summary>
    ''' <returns>The Case Data</returns>
    Public Function GetCaseJsonObject() As JObject
        If _caseJsonObject Is Nothing Then
            _caseJsonObject = Newtonsoft.Json.Linq.JObject.Parse(CaseData)
        End If

        Return _caseJsonObject
    End Function

    ''' <summary>
    ''' Return field value in Legal case data
    ''' </summary>
    ''' <typeparam name="T">The field Type</typeparam>
    ''' <param name="path">The json query path</param>
    ''' <returns>The field value</returns>
    Public Function GetFieldValue(Of T)(path As String) As T
        If String.IsNullOrEmpty(path) Then
            Return Nothing
        End If

        Dim jObject = GetCaseJsonObject()

        Dim token = jObject.SelectToken(path)
        If token IsNot Nothing Then
            Return CTypeDynamic(Of T)(token)
        End If

        Return Nothing
    End Function

    Public Shared Function GetCase(bble As String) As LegalCase
        Using ctx As New PortalEntities
            Return ctx.LegalCases.Find(bble)
        End Using
    End Function

    Public Shared Function GetCaseOwner(bble As String) As String
        Using ctx As New PortalEntities
            Dim lcase = ctx.LegalCases.Where(Function(a) a.BBLE = bble).Select(Function(lc) New With {lc.BBLE, lc.ResearchBy, lc.Attorney}).FirstOrDefault

            If lcase IsNot Nothing Then
                If String.IsNullOrEmpty(lcase.Attorney) Then
                    Return lcase.ResearchBy
                Else
                    Return lcase.Attorney
                End If
            End If

            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Get the Case by index number if there are two cases have same 
    ''' index number then take the frist one
    ''' </summary>
    ''' <param name="indexNum"></param>
    ''' <returns></returns>
    Public Shared Function GetLegalCaseByFcIndex(indexNum As String) As LegalCase
        If (String.IsNullOrEmpty(indexNum)) Then
            Return Nothing
        End If
        Dim eCortIndex = IndexNumberFormat(indexNum)
        Using ctx As New PortalEntities
            Dim lcases = ctx.LegalCases.Where(Function(c) Not String.IsNullOrEmpty(c.FCIndexNum)).Where(Function(c) c.FCIndexNum.Contains(eCortIndex)).ToList()
            For Each c In lcases
                Dim legalCaseIndex = IndexNumberFormat(c.FCIndexNum)
                If (legalCaseIndex = eCortIndex) Then
                    Return c
                End If
            Next
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Remove index number to no leading zero like "001234/2015" to "1234/2015"
    ''' </summary>
    ''' <param name="IndexNumber"></param>
    ''' <returns></returns>
    Public Shared Function IndexNumberFormat(IndexNumber As String) As String
        If (String.IsNullOrEmpty(IndexNumber)) Then
            Return Nothing
        End If
        Dim ZeroIndex = 0
        For i = 0 To IndexNumber.Length
            If (IndexNumber.Chars(i) <> "0") Then
                ZeroIndex = i
                Exit For
            End If
        Next
        Return IndexNumber.Substring(ZeroIndex)
    End Function

    ''' <summary>
    ''' Decode index numbe make it as uqniue ID decode 123/1234 to 000123/1234
    ''' </summary>
    ''' <param name="IndexNumber">input number need to decode</param>
    ''' <returns>Decoded quniue index number return nothing means input ilegal index number</returns>
    Public Shared Function DeCodeIndexNumber(IndexNumber As String) As String
        If (String.IsNullOrEmpty(IndexNumber) Or IndexNumber.IndexOf("/") < 0) Then
            Return Nothing
        End If
        Dim a = IndexNumber.Split("/")
        Return String.Join("/", CInt(a(0)).ToString("D6"), a(1))

    End Function
    Public Shared Sub UpdateStatus(bble As String, status As LegalCaseStatus, updateBy As String)
        'update legal case status
        Dim lc = GetCase(bble)
        lc.Status = status
        lc.SaveData(updateBy)
    End Sub

    Public Shared Function InLegal(bble As String) As Boolean
        Return LegalCase.GetCase(bble) IsNot Nothing
    End Function

    Public Shared Function GetCaseList(status As LegalCaseStatus) As List(Of LegalCase)
        Using ctx As New PortalEntities
            Return ctx.LegalCases.Where(Function(lc) lc.Status = status).ToList
        End Using
    End Function

    Public Shared Function GetAllCases() As List(Of LegalCase)
        Using ctx As New PortalEntities
            Return ctx.LegalCases.ToList
        End Using
    End Function

    Public Shared Function GetFollowUpCases() As List(Of LegalCase)
        Using ctx As New PortalEntities
            Return ctx.LegalCases.Where(Function(lc) lc.FollowUp.HasValue).OrderByDescending(Function(lc) lc.FollowUp).ToList
        End Using
    End Function

    Public Shared Function GetFollowUpCaseByUser(userName As String) As List(Of LegalCase)
        Return GetFollowUpCases().Where(Function(lc) (lc.ResearchBy = userName Or lc.Attorney = userName)).ToList
    End Function

    Public Shared Function GetLightCaseList(status1 As LegalCaseStatus) As List(Of LegalCase)
        Using ctx As New PortalEntities
            Dim result = From lCase In ctx.LegalCases.Where(Function(lc) lc.Status = status1)
                         Select lCase.BBLE, lCase.CaseName, lCase.ResearchBy, lCase.Attorney, lCase.Status, lCase.LegalStatus, lCase.FollowUp, lCase.SaleDate, lCase.SecondaryTypes, lCase.UpdateDate, lCase.UpdateBy, lCase.CreateBy, lCase.CreateDate

            Return result.AsEnumerable.Select(Function(lcase)
                                                  Return New LegalCase With {
                                                    .BBLE = lcase.BBLE,
                                                    .Attorney = lcase.Attorney,
                                                    .CaseName = lcase.CaseName,
                                                    .CreateBy = lcase.CreateBy,
                                                    .CreateDate = lcase.CreateDate,
                                                    .FollowUp = lcase.FollowUp,
                                                    .ResearchBy = lcase.ResearchBy,
                                                    .LegalStatus = lcase.LegalStatus,
                                                    .SaleDate = lcase.SaleDate,
                                                    .SecondaryTypes = lcase.SecondaryTypes,
                                                    .UpdateBy = lcase.UpdateBy,
                                                    .UpdateDate = lcase.UpdateDate,
                                                    .Status = lcase.Status
                                                  }
                                              End Function).ToList
        End Using
    End Function

    Public Shared Function GetLightCaseList(status1 As LegalCaseStatus, userName As String) As List(Of LegalCase)
        Using ctx As New PortalEntities
            Dim result = From lCase In ctx.LegalCases.Where(Function(lc) lc.Status = status1 AndAlso (lc.ResearchBy = userName Or lc.Attorney = userName))
                         Select lCase.BBLE, lCase.CaseName, lCase.ResearchBy, lCase.Attorney, lCase.Status, lCase.LegalStatus, lCase.FollowUp, lCase.SaleDate, lCase.SecondaryTypes, lCase.UpdateDate, lCase.UpdateBy, lCase.CreateBy, lCase.CreateDate

            Return result.AsEnumerable.Select(Function(lcase)
                                                  Return New LegalCase With {
                                                    .BBLE = lcase.BBLE,
                                                    .Attorney = lcase.Attorney,
                                                    .CaseName = lcase.CaseName,
                                                    .CreateBy = lcase.CreateBy,
                                                    .CreateDate = lcase.CreateDate,
                                                    .FollowUp = lcase.FollowUp,
                                                    .ResearchBy = lcase.ResearchBy,
                                                    .LegalStatus = lcase.LegalStatus,
                                                    .SaleDate = lcase.SaleDate,
                                                    .SecondaryTypes = lcase.SecondaryTypes,
                                                    .UpdateBy = lcase.UpdateBy,
                                                    .UpdateDate = lcase.UpdateDate,
                                                    .Status = lcase.Status
                                                  }
                                              End Function).ToList
        End Using
    End Function

    Public Shared Function GetCaseList(status1 As LegalCaseStatus, userName As String) As List(Of LegalCase)
        Using ctx As New PortalEntities
            Return ctx.LegalCases.Where(Function(lc) lc.Status = status1 AndAlso (lc.ResearchBy = userName Or lc.Attorney = userName)).ToList
        End Using
    End Function

    ''' <summary>
    ''' Return list of selected Secondary Action
    ''' </summary>
    ''' <returns>The list of Secondary Action</returns>
    Public Function GetSecondaryActions() As List(Of SecondaryAction)
        Dim actionTypes = DataStatu.LoadAllDataStatus("LegalSecondaryType")

        Dim result = New List(Of SecondaryAction)

        Dim tags = GetFieldValue(Of JArray)("SecondaryTypes")

        If tags IsNot Nothing AndAlso tags.Count > 0 Then
            Dim types = tags.ToObject(Of String())()
            For Each type In actionTypes
                If types.Contains(type.Status) Then
                    Dim action As New SecondaryAction
                    action.Id = type.Status
                    action.Type = type.Name

                    If Not String.IsNullOrEmpty(type.Description) Then
                        Dim dataNames = type.Description.Split("|")
                        action.Defendant = GetFieldValue(Of String)(dataNames(0))
                        action.DefendantAttorney = GetFieldValue(Of String)(dataNames(1))
                        action.Plaintiff = GetFieldValue(Of String)(dataNames(2))
                        action.PlaintiffAttorney = GetFieldValue(Of String)(dataNames(3))
                    End If

                    result.Add(action)
                End If
            Next
        End If

        Return result
    End Function

    ''' <summary>
    ''' Secondary Action Object
    ''' </summary>
    Class SecondaryAction

        ''' <summary>
        ''' Action Id
        ''' </summary>
        ''' <returns></returns>
        Public Property Id As Integer
        ''' <summary>
        ''' The Action Name
        ''' </summary>
        ''' <returns></returns>
        Public Property Type As String
        ''' <summary>
        ''' The Defendant Info
        ''' </summary>
        ''' <returns></returns>
        Public Property Defendant As String
        ''' <summary>
        ''' The Plantiff Info
        ''' </summary>
        ''' <returns></returns>
        Public Property Plaintiff As String
        ''' <summary>
        ''' The Defendant Attorney Info
        ''' </summary>
        ''' <returns></returns>
        Public Property DefendantAttorney As String
        ''' <summary>
        ''' The Plantiff Attorney Info
        ''' </summary>
        ''' <returns></returns>
        Public Property PlaintiffAttorney As String

    End Class

End Class

Public Enum LegalCaseStatus
    <Description("New Cases")>
    ManagerPreview = 0
    <Description("Research")>
    LegalResearch = 1
    <Description("Review")>
    ManagerAssign = 2
    <Description("In Court")>
    AttorneyHandle = 3
    <Description("Closed")>
    Closed = 4
End Enum

Public Enum DataStatus2
    <Description("No current action")>
    NoAction = 1
    <Description("S&C/LP")>
    SCLP = 2
    <Description("RJI")>
    RJI = 3
    <Description("Settlement Conf")>
    SettlementConf = 4

    <Description("O/REF")>
    OREF = 5

    <Description("FCS Conference")>
    FCSConference = 151
    <Description("Released from FCS")>
    ReleasedfromFCS = 152
    <Description("Status Conference")>
    StatusConference = 153
    <Description("OSC Pending")>
    OSCPending = 154
    <Description("O/R Submitted")>
    ORSubmitted = 155
    <Description("O/R Granted")>
    ORGranted = 156
    <Description("O/R Entered")>
    OREntered = 157
    <Description("O/R Denied")>
    ORDenied = 158
    <Description("O/R Withdrawn")>
    ORWithdrawn = 159
    <Description("O/R Vacated")>
    ORVacated = 160

    <Description("Judgment Submitted")>
    JudgmentSubmitted = 10
    <Description("Judgment Granted")>
    JudgmentGranted = 11
    <Description("Judgment Entered")>
    JudgmentEntered = 12
    <Description("Judgment Denied")>
    JudgmentEnteredDenied = 221
    <Description("Judgment Withdrawn")>
    JudgmentWithdrawn = 222
    <Description("Judgment Vacated")>
    JudgmentVacated = 2223

    <Description("Dismissed w Prejudice")>
    DismissedWithPrejudice = 8
    <Description("Dismissed w/o Prejudice")>
    DismissedWithoutPrejudice = 9


    <Description("Sale Date")>
    SaleDate = 7
    <Description("Sale Cancelled")>
    SaleCancelled = 71
    <Description("Sale Rescheduled")>
    SaleRescheduled = 72

    <Description("Restored To calendar")>
    Restored2Calendar = 10101
    <Description("motion To restore submitted")>
    MotionToTestoreSubmitted = 10102
    <Description("motion To restore withdrawn")>
    MotionToTestoreWithdrawn = 10103
    <Description("ref deed recorded")>
    RefDeedRecorded = 10104
End Enum


Public Enum LegalSencdaryType2
    <Description("Order To show Case")>
    OSC = 1
    <Description("Partitions")>
    Partitions = 2
    <Description("QTA")>
    QTA = 3
    <Description("DeedReversions")>
    DeedReversions = 4
    <Description("Specific Performance")>
    SpecificPerformance = 5
    <Description("Misc. actions")>
    Other = 6
End Enum
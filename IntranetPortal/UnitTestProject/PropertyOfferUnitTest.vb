﻿Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports IntranetPortal
Imports System.IO
Imports Newtonsoft.Json.Linq
Imports IntranetPortal.Data

''' <summary>
''' The UnitTest for PropertyOffer function
''' </summary>
<TestClass()> Public Class PropertyOfferUnitTest

    Dim BBLE As String = "4089170024"

    ''' <summary>
    ''' Generate document package testing
    ''' </summary>
    <TestMethod()> Public Sub GeneratePackage_ReturnLink()
        Dim jsData = <string>
                         <![CDATA[
                         {"BusinessData":{"OfferId":8237,"FormItemId":9253,"BBLE":"1017700038 ","Title":"2236 3 AVE, Manhattan,NY 10035","Owner":"Chris Yan","OfferType":"Short Sale","Status":0,"CreateDate":"2016-07-27T11:43:28.277","CreateBy":"Chris Yan","UpdateBy":"Chris Yan","UpdateDate":"2016-07-27T11:43:30.053","ContractSeller1":null,"ContractSeller2":null,"ContractSeller3":null,"ContractPrice":0,"ContractDownPay":0,"Name":null},"DataId":9253,"FormName":"PropertyOffer","FormData":null,"Tag":"1017700038","UpdateDate":"2016-07-27T11:43:30.037","UpdateBy":"Chris Yan","CreateDate":"2016-07-27T11:43:28.263","CreateBy":"Chris Yan","assignCrop":{"BBLE":"1017700038","newOfferId":8237,"Name":"RonTeam","isWellsFargo":false,"Signer":"Magda Brillite","signers":["Magda Brillite","Ron Borovinsky","Princess Simeon"],"Crop":"Hancock 10 Holdings LLC","CropData":{"EntityId":2068,"CorpName":"Hancock 10 Holdings LLC","Address":"50-04 102 Street, Corona, NY 11368","PropertyAssigned":"2082 DEAN ST, Brooklyn,NY 11233","FillingDate":"2014-04-17T00:00:00","Signer":"Magda Brillite","Office":"RonTeam","EIN":null,"AssignOn":"2016-07-06T17:02:01.013","ReceivedOn":null,"SubmittedtoAcris":null,"SubmittedOn":null,"RecordedOn":null,"Notes":null,"EINFile":null,"File2":"/CorporationEntity/Hancock 10 Holdings LLC/Corporation.pdf","File3":"/CorporationEntity/Hancock 10 Holdings LLC/Corporation2.pdf","CreateBy":null,"CreateTime":null,"UpdateBy":"Chris Yan","UpdateTime":"2016-07-06T17:02:01.013","Status":"Available","OfficeName":"Patchen","BBLE":null,"AppId":1,"IsReserve":false,"AssignedOutDays":0,"buyerAttorney":"Henry Graham, Esq.","buyerAttorneyObj":{"ContactId":930,"Name":"Henry Graham, Esq.","OfficeNO":"(718) 793-1311","Email":"hgraham@henrygrahamlaw.com","CorpName":null,"Office":"80-02 Kew Gardens Road, Suite 300, Kew Gardens, NY 11415","Cell":"","Address":null,"CreateBy":"Gladys Best","CreateDate":"2015-08-05T13:12:44.387","Type":null,"Extension":null,"Fax":"","GroupId":3,"CustomerService":"","Disable":null,"CustomerServiceFax":null,"Notes":null,"AppId":1,"Corps":null}}},"DealSheet":{"ContractOrMemo":{"Sellers":[{"$$hashKey":"object:35","active":false,"isCorp":false,"FirstName":"Shamrock","LastName":"Asset","DOB":"10/10/1980","SSN":"114541245","Phone":"8523654521","AdlPhone":"4511245454","Email":"test@test.com","MailNumber":"2236","MailStreetName":"3 ave","MailCity":"Manhattan","MailState":"NY","MailZip":"10035","Bankruptcy":false,"Bankaccount":false,"ActiveMilitar":false,"TaxReturn":false,"Employed":"Employed","Paystubs":false,"Name":"Shamrock Asset","Address":"2236 3 AVE, Manhattan,NY 10035","PropertyAddress":"2236 3 AVE, Manhattan,NY 10035","sellerAttorney":"Jason J. Rebhun Esq.","sellerAttorneyObj":{"ContactId":950,"Name":"Jason J. Rebhun Esq.","OfficeNO":"(646) 201-9392","Email":"jason@jasonrebhun.com","CorpName":null,"Office":null,"Cell":"(845) 821-1589","Address":"225 Broadway 38th Floor, New York NY 10007","CreateBy":null,"CreateDate":null,"Type":null,"Extension":null,"Fax":null,"GroupId":3,"CustomerService":null,"Disable":null,"CustomerServiceFax":null,"Notes":null,"AppId":1,"Corps":null}},{"isCorp":false,"$$hashKey":"object:100","active":false,"FirstName":"Miriam","LastName":"Lofffredo","DOB":"04/05/1970","SSN":"121121545","Phone":"4561244545","AdlPhone":"1214545454","MailNumber":"2236","MailStreetName":"3 Ave","MailCity":"Manhattan","MailState":"NY","MailZip":"10035","Bankruptcy":true,"Bankaccount":true,"BankruptcyChapter":"Chapter 7","ActiveMilitar":true,"TaxReturn":true,"Employed":"Employed","Paystubs":true,"Name":"Miriam Lofffredo","Address":"2236 3 AVE, Manhattan,NY 10035","PropertyAddress":"2236 3 AVE, Manhattan,NY 10035","sellerAttorney":"David Alishaev, Esq.","sellerAttorneyObj":{"ContactId":931,"Name":"David Alishaev, Esq.","OfficeNO":"(718) 459-2030","Email":null,"CorpName":null,"Office":null,"Cell":"","Address":"100-15 Queens Blvd, Suite 203, Forrest Hills, NY 11375","CreateBy":"Gladys Best","CreateDate":"2015-08-05T14:45:36.07","Type":null,"Extension":null,"Fax":"","GroupId":3,"CustomerService":"","Disable":null,"CustomerServiceFax":null,"Notes":null,"AppId":1,"Corps":null}},{"isCorp":false,"$$hashKey":"object:124","active":true,"FirstName":"Lonelle","LastName":"Jones","DOB":"02/03/1975","SSN":"124545454","Phone":"4563258965","MailNumber":"2412","MailStreetName":"Pacific st","MailCity":"Forest hill","MailState":"NY","MailZip":"11654","Bankruptcy":false,"TaxReturn":false,"Bankaccount":false,"ActiveMilitar":false,"Employed":"Self-Employed","Paystubs":false,"Name":"Lonelle Jones","Address":"2236 3 AVE, Manhattan,NY 10035","PropertyAddress":"2236 3 AVE, Manhattan,NY 10035","sellerAttorney":"Jeffrey L Weinstein, Esq.","sellerAttorneyObj":{"ContactId":941,"Name":"Jeffrey L Weinstein, Esq.","OfficeNO":"(212) 693-3737","Email":null,"CorpName":null,"Office":null,"Cell":"","Address":"225 Broadway, Suite 3800, NY, NY 1007","CreateBy":"Gladys Best","CreateDate":"2015-08-10T12:45:23.88","Type":null,"Extension":null,"Fax":"(212) 693-2020","GroupId":3,"CustomerService":"","Disable":null,"CustomerServiceFax":null,"Notes":null,"AppId":1,"Corps":null}}],"Buyers":[{}],"Buyer":{"EntityId":2068,"CorpName":"Hancock 10 Holdings LLC","Address":"50-04 102 Street, Corona, NY 11368","PropertyAssigned":"2082 DEAN ST, Brooklyn,NY 11233","FillingDate":"2014-04-17T00:00:00","Signer":"Magda Brillite","Office":"RonTeam","EIN":null,"AssignOn":"2016-07-06T17:02:01.013","ReceivedOn":null,"SubmittedtoAcris":null,"SubmittedOn":null,"RecordedOn":null,"Notes":null,"EINFile":null,"File2":"/CorporationEntity/Hancock 10 Holdings LLC/Corporation.pdf","File3":"/CorporationEntity/Hancock 10 Holdings LLC/Corporation2.pdf","CreateBy":null,"CreateTime":null,"UpdateBy":"Chris Yan","UpdateTime":"2016-07-06T17:02:01.013","Status":"Available","OfficeName":"Patchen","BBLE":null,"AppId":1,"IsReserve":false,"AssignedOutDays":0,"buyerAttorney":"Henry Graham, Esq.","buyerAttorneyObj":{"ContactId":930,"Name":"Henry Graham, Esq.","OfficeNO":"(718) 793-1311","Email":"hgraham@henrygrahamlaw.com","CorpName":null,"Office":"80-02 Kew Gardens Road, Suite 300, Kew Gardens, NY 11415","Cell":"","Address":null,"CreateBy":"Gladys Best","CreateDate":"2015-08-05T13:12:44.387","Type":null,"Extension":null,"Fax":"","GroupId":3,"CustomerService":"","Disable":null,"CustomerServiceFax":null,"Notes":null,"AppId":1,"Corps":null}},"contractPrice":"500000","downPayment":"10000"},"Deed":{"Sellers":[{"$$hashKey":"object:35","active":false,"isCorp":false,"FirstName":"Shamrock","LastName":"Asset","DOB":"10/10/1980","SSN":"114541245","Phone":"8523654521","AdlPhone":"4511245454","Email":"test@test.com","MailNumber":"2236","MailStreetName":"3 ave","MailCity":"Manhattan","MailState":"NY","MailZip":"10035","Bankruptcy":false,"Bankaccount":false,"ActiveMilitar":false,"TaxReturn":false,"Employed":"Employed","Paystubs":false,"Name":"Shamrock Asset","Address":"2236 3 AVE, Manhattan,NY 10035","PropertyAddress":"2236 3 AVE, Manhattan,NY 10035"},{"isCorp":false,"$$hashKey":"object:100","active":true,"FirstName":"Miriam","LastName":"Lofffredo","DOB":"04/05/1970","SSN":"121121545","Phone":"4561244545","AdlPhone":"1214545454","MailNumber":"2236","MailStreetName":"3 Ave","MailCity":"Manhattan","MailState":"NY","MailZip":"10035","Bankruptcy":true,"Bankaccount":true,"BankruptcyChapter":"Chapter 7","ActiveMilitar":true,"TaxReturn":true,"Employed":"Employed","Paystubs":true,"Name":"Miriam Lofffredo","Address":"2236 3 AVE, Manhattan,NY 10035","PropertyAddress":"2236 3 AVE, Manhattan,NY 10035"},{"isCorp":false,"$$hashKey":"object:124","active":false,"FirstName":"Lonelle","LastName":"Jones","DOB":"02/03/1975","SSN":"124545454","Phone":"4563258965","MailNumber":"2412","MailStreetName":"Pacific st","MailCity":"Forest hill","MailState":"NY","MailZip":"11654","Bankruptcy":false,"TaxReturn":false,"Bankaccount":false,"ActiveMilitar":false,"Employed":"Self-Employed","Paystubs":false,"Name":"Lonelle Jones","Address":"2236 3 AVE, Manhattan,NY 10035","PropertyAddress":"2236 3 AVE, Manhattan,NY 10035"}],"PropertyAddress":"2236 3 AVE, Manhattan,NY 10035","Buyer":{"EntityId":5,"BBLE":null,"CorpName":"508 Van Buren St INC ","Address":"116-55 Queens Blvd, Ste 206, Forest Hills, NY 11375","PropertyAssigned":null,"FillingDate":"2012-07-03T00:00:00","Signer":"The Corporation ","OfficeName":"Forest Hills","Office":"GendinTeam","EIN":"45-5629177","AssignOn":null,"ReceivedOn":null,"SubmittedtoAcris":null,"SubmittedOn":null,"RecordedOn":null,"Notes":null,"EINFile":null,"File2":null,"File3":null,"CreateBy":null,"CreateTime":null,"UpdateBy":null,"UpdateTime":null,"Status":"Deed Corp","AppId":1,"Active":true,"Properties":null},"EntityId":5},"CorrectionDeed":{"Sellers":[{"$$hashKey":"object:47","active":true,"Name":"Shamrock Asset","SSN":"454-54-5454","Address":"2236 3 AVE, Manhattan,NY 10035"}],"Buyers":[{"$$hashKey":"object:50","active":true,"Name":"Shamrock Test","SSN":"787-85-4545","Address":"2236 3 AVE, Manhattan,NY 10035"}],"PropertyAddress":"2236 3 AVE, Manhattan,NY 10035"},"GivingPOA":{"Name":"Miriam Lofffredo","Address":"2236 3 AVE, Manhattan,NY 10035"},"ReceivingPOA":{"name":"Testing","address":"2236 3 AVE, Manhattan,NY 10035"}},"Type":"Short Sale","BBLE":"1017700038","PropertyAddress":"2236 3 AVE, Manhattan,NY 10035","DeadType":{"Contract":true,"Memo":true,"Deed":true,"CorrectionDeed":true,"POA":true, "ShortSale":true},"SsCase":{"PropertyInfo":{"Owners":[{"isCorp":false,"$$hashKey":"object:35","active":true,"FirstName":"Shamrock","LastName":"Asset","DOB":"10/10/1980","SSN":"114541245","Phone":"8523654521","AdlPhone":"4511245454","Email":"test@test.com","MailNumber":"2236","MailStreetName":"3 ave","MailCity":"Manhattan","MailState":"NY","MailZip":"10035","Bankruptcy":false,"Bankaccount":false,"ActiveMilitar":false,"TaxReturn":false,"Employed":"Employed","Paystubs":false,"Name":"Shamrock Asset","Address":"2236 3 AVE, Manhattan,NY 10035","PropertyAddress":"2236 3 AVE, Manhattan,NY 10035"},{"isCorp":false,"$$hashKey":"object:100","active":false,"FirstName":"Miriam","LastName":"Lofffredo","DOB":"04/05/1970","SSN":"121121545","Phone":"4561244545","AdlPhone":"1214545454","MailNumber":"2236","MailStreetName":"3 Ave","MailCity":"Manhattan","MailState":"NY","MailZip":"10035","Bankruptcy":true,"Bankaccount":true,"BankruptcyChapter":"Chapter 7","ActiveMilitar":true,"TaxReturn":true,"Employed":"Employed","Paystubs":true,"Name":"Miriam Lofffredo","Address":"2236 3 AVE, Manhattan,NY 10035","PropertyAddress":"2236 3 AVE, Manhattan,NY 10035"},{"isCorp":false,"$$hashKey":"object:124","active":false,"FirstName":"Lonelle","LastName":"Jones","DOB":"02/03/1975","SSN":"124545454","Phone":"4563258965","MailNumber":"2412","MailStreetName":"Pacific st","MailCity":"Forest hill","MailState":"NY","MailZip":"11654","Bankruptcy":false,"TaxReturn":false,"Bankaccount":false,"ActiveMilitar":false,"Employed":"Self-Employed","Paystubs":false,"Name":"Lonelle Jones","Address":"2236 3 AVE, Manhattan,NY 10035","PropertyAddress":"2236 3 AVE, Manhattan,NY 10035"}]},"CaseData":{},"Mortgages":[{"$$hashKey":"object:38","LenderName":"21st Mortgage","Lender":{"ContactId":847,"Name":"21st Mortgage","OfficeNO":"Ext. 5555","Email":null,"CorpName":"21st Mortgage","Office":null,"Cell":null,"Address":null,"CreateBy":"LenderImport","CreateDate":"2015-07-29T00:00:00","Type":4,"Extension":null,"Fax":"877-830-3100","GroupId":5,"CustomerService":"800-955-0021","Disable":null,"CustomerServiceFax":"877-830-3100","Notes":null,"AppId":1,"Corps":null},"LenderId":847,"Loan":"7878787878","LoanAmount":"520000"},{"$$hashKey":"object:540","LenderName":"AllianceOne","Lender":{"ContactId":848,"Name":"AllianceOne","OfficeNO":"866-672-3308","Email":null,"CorpName":"AllianceOne","Office":null,"Cell":null,"Address":null,"CreateBy":"LenderImport","CreateDate":"2015-07-29T00:00:00","Type":4,"Extension":null,"Fax":"651-452-1263","GroupId":5,"CustomerService":"866-672-3308","Disable":null,"CustomerServiceFax":"651-452-1263","Notes":null,"AppId":1,"Corps":null},"LenderId":848,"Loan":"45521454654","LoanAmount":"40000"},{"$$hashKey":"object:744","LenderName":"ASC","Lender":{"ContactId":849,"Name":"ASC","OfficeNO":"866-903-1053","Email":null,"CorpName":"ASC","Office":null,"Cell":null,"Address":null,"CreateBy":"LenderImport","CreateDate":"2015-07-29T00:00:00","Type":4,"Extension":null,"Fax":"866-969-0103","GroupId":5,"CustomerService":"800-842-7654","Disable":null,"CustomerServiceFax":"866-917-1877","Notes":null,"AppId":1,"Corps":null},"LenderId":849,"Loan":"789784545","LoanAmount":"45000"}]},"Status":2}
                         ]]>
                     </string>

        Dim path = "G:\Working Folder\WebApp\IntranetPortalGit\IntranetPortal\IntranetPortal\App_Data\OfferDoc"
        Dim destPath = "G:\Working Folder\WebApp\IntranetPortalGit\IntranetPortal\IntranetPortal\TempDataFile\OfferDoc\"

        Dim data = CType(JObject.Parse(jsData), JObject)

        Dim link = PropertyOfferManage.GeneratePackage(BBLE, data, path, destPath)

        Assert.IsTrue(link.Contains(BBLE))
        Assert.IsTrue(File.Exists(destPath & link))
    End Sub

    <TestMethod()>
    Public Sub SSAcceptedLastWeek_returnListOfOffer()
        Dim offer = PropertyOfferManage.GetSSAcceptedOfferLastWeek("*", Nothing, Nothing)
        Assert.IsTrue(offer.Count > 0)
    End Sub

    <TestMethod()>
    Public Sub PendingNewOffer_returnListOfOffer()
        Dim offer = PropertyOffer.GetPendingOF({"Chris Yan"})
        Assert.IsTrue(offer.Count > 0)
    End Sub

    <TestMethod()>
    Public Sub CompletedNewOfferDue_returnListOfOffer()
        Dim offer = PropertyOfferManage.CompletedNewOfferDue("*")
        Assert.IsTrue(offer.Count > 0)
    End Sub

    <TestMethod()>
    Public Sub InprocessNewOfferDue_returnListOfOffer()
        Dim offer = PropertyOfferManage.InProcessNewOfferDue("*")
        Assert.IsTrue(offer.Count > 0)
    End Sub

    <TestMethod()> Public Sub UpdateFields_ReturnUpdatedData()
        Dim formId = 191
        Dim item = FormDataItem.Instance(formId)

        Dim offer As New PropertyOffer

        offer.UpdateFields(item)

        Assert.IsNotNull(offer.Status)
    End Sub

    <TestMethod()> Public Sub NotifyTeamManager_sendEmail()
        Dim rule As New IntranetPortal.RulesEngine.NewOfferNotifyRule
        rule.Execute()
    End Sub

    <TestMethod()> Public Sub NewOfferDueNotifyRule_sendEmail()
        Dim rule As New IntranetPortal.RulesEngine.NewOfferNotifyRule
        rule.IsWeekly = False
        rule.Execute()
    End Sub

    <TestMethod()> Public Sub UpdateAudit_ReturnData()
        Dim prop = PropertyOffer.GetOffer("4070570032")
        Dim seller1 = prop.ContractSeller1
        Dim seller2 = prop.ContractSeller2
        Dim seller3 = prop.ContractSeller3
        Dim cp = prop.ContractPrice
        Dim dp = prop.ContractDownPay

        prop.ContractSeller1 = "Test Seller1"
        prop.ContractSeller2 = "Test Seller2"
        prop.ContractSeller3 = "Test Seller3"
        prop.ContractPrice = 10000.0
        prop.ContractDownPay = 6000

        prop.SaveData("UnitTest")

        Dim logs = AuditLog.GetLogs("PropertyOffer", prop.OfferId).ToList
        Assert.IsTrue(logs IsNot Nothing)
        Assert.IsTrue(logs.Where(Function(a) a.UserName = "UnitTest" AndAlso a.RecordId = prop.OfferId).Count > 0)
        prop.ContractSeller1 = seller1
        prop.ContractSeller2 = seller2
        prop.ContractSeller3 = seller3
        prop.ContractPrice = cp
        prop.ContractDownPay = dp
        prop.SaveData("UnitTest")

        For Each log In logs
            log.Delete()
        Next
    End Sub

End Class
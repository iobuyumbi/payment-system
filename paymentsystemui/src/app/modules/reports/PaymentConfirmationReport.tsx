import React, { useEffect, useRef, useState } from "react";
import ApexCharts, { ApexOptions } from "apexcharts";
import * as XLSX from 'xlsx';
import saveAs from "file-saver";
import { PageLink } from "../../../_metronic/layout/core";
import { Content } from "../../../_metronic/layout/components/content";
import { KTCard, KTCardBody } from "../../../_metronic/helpers";
import CustomTable from "../../../_shared/CustomTable/Index";
import DeductiblePaymentStats from "./partials/DeductiblePaymentStats";
import ReportService from "../../../services/ReportService";
import { PaymentDeductibleModel } from '../../../_models/deductible-model';
const reportService = new ReportService();
const profileBreadCrumbs: Array<PageLink> = [
  {
    title: "Reports",
    path: "/reports",
    isSeparator: false,
    isActive: true,
  },
];
const PaymentConfirmationReport = () => {
  const [rowData, setRowData] = useState<any>();
  const[stats, setStats] = useState<any>();
  const [colDefs, setColDefs] = useState<any>([
    { field: "systemId", flex: 1, headerName: "System Id", filter: true },
    { field: "farmerLoansDeductionsLc", flex: 2, headerName: "Loan Deductions" },
    { field: "farmerPayableEarningsLc", flex: 2, headerName: "Payable Earnings" },
    { field: "farmerEarningsShareLc", flex: 2, headerName: "Farmer Earnings" },
    // { field: "isPaymentComplete", flex: 1, cellRenderer: StatusComponent },
    // { field: "Action", flex: 1, cellRenderer: CustomActionComponent },
  ]);

  const handleExport = () => {
    const workbook = XLSX.utils.book_new();
    // Map data to custom headers
    const mappedData = rowData.map((row: any) => ({
      "System Id": row.systemId,
      "First name": row.firstName,
      "Other names": row.otherNames,
      Country: row.country.countryName,
      Mobile: row.mobile,
      "Payment phone number": row.paymentPhoneNumber,
      "Access to mobile": row.accessToMobile,
      "County/Region": row.adminLevel1.countyName,
      "Sub-County/District": row.adminLevel2.subCountyName,
      "Ward/Sub-County/County": row.adminLevel3.wardName,
      Village: row.village,
      "Alternate contact number": row.alternateContactNumber,
      "Beneficiary id": row.beneficiaryId,
      "Birth month": row.birthMonth,
      "Birth year": row.birthYear,
      Cooperative: row.cooperativeName,
      Email: row.email,
      "Enumeration date": row.enumerationDate,
      Gender: row.gender === 1 ? "Male" : row.gender === 2 ? "Female" : "Other",
      "Has disability": row.hasDisability,
      "Is farmer phone owner?": row.isFarmerPhoneOwner,
      "Participant id": row.participantId,
      "Phone owner address": row.phoneOwnerAddress,
      "Phone owner name": row.phoneOwnerName,
      "Phone owner national id": row.phoneOwnerNationalId,
    }));

    const worksheet = XLSX.utils.json_to_sheet(mappedData);
    XLSX.utils.book_append_sheet(workbook, worksheet, "Sheet1");
    const blob = new Blob(
      [XLSX.write(workbook, { bookType: "xlsx", type: "array" })],
      {
        type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
      }
    );
    saveAs(blob, "farmers.xlsx");
  };

  const bindImportHistory = async () => {
    try {  const data: any = {
      pageNumber: 1,
      pageSize: 10000,
  };
        const response = await reportService.getPaymentConfirmationStats(data);
        
        if (response) {
        
            setRowData(response.paymentDeductibles);
            setStats(response.paymentStatus);
        }
    } catch (error) {
        console.error("Failed to bind import history:", error);
    }
};
useEffect(() => {

  bindImportHistory();
}, []);
  return (
    <Content>
      <div className="card">
        <div className="card-header border d-flex align-items-center justify-content-between">
          <h3 className="card-title fw-bold text-gray-900">
            {" "}
            Payments Confirmation Report
          </h3>
        </div>
        <div className="mx-3">
          <DeductiblePaymentStats stats={stats}/>
        </div>
      </div>
      <KTCard className="shadow my-3">
        <KTCardBody className="m-0 p-0">
          <CustomTable
            rowData={rowData}
            colDefs={colDefs}
            header="Transactions"
            addBtnText={""}
            importBtnText={""}
            addBtnLink={""}
            showExport={true}
            handleExport={handleExport}
            showDateBox={true}
          />
        </KTCardBody>
      </KTCard>
    </Content>
  );
};

export { PaymentConfirmationReport };

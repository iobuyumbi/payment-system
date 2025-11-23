import { useEffect, useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom';
import { KTCard } from '../../../_metronic/helpers';
import { Content } from '../../../_metronic/layout/components/content';
import CustomTable from '../../../_shared/CustomTable/Index';
import FarmerService from '../../../services/FarmerService';
import moment from 'moment';
import config from '../../../environments/config';
import KycService from '../../../services/KycService';
import InfoCard from './partials/InfoCard';
import PaymentBatchService from '../../../services/PaymentBatchService';
import { IConfirmModel } from '../../../_models/confirm-model';
import { ConfirmBox } from '../../../_shared/Modals/ConfirmBox';
import { toast } from 'react-toastify';
import ImportService from '../../../services/ImportService';
import ExcelExportService from '../../../services/ExportService';

const farmerService = new FarmerService();
const importService = new ImportService();
const paymentBatchService = new PaymentBatchService();
const exportService = new ExcelExportService();
const PrePaymentKyc = () => {
const {batchId}= useParams();
  const [rowData, setRowData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  const [importHistory, setImportHistory] = useState<any[]>([]);
  const navigate = useNavigate();

  const [showImport, setShowImport] = useState<boolean>(false);
 

  const StatusComponent = (props: any) => {
    return <div className="py-1">
      {props.data.farmer.isFarmerVerified === true && <div className='badge fs-7 badge-light-success  fw-normal'>Yes</div>}
      {props.data.farmer.isFarmerVerified === false && <div className='badge fs-7 badge-light-danger  fw-normal'>No</div>}
    </div>;
  };

  const FarmerComponent = (props: any) => {
   
    return <div>
    <div>{props.data.farmer.fullName }</div>
   
  </div>;
  };
  const MobileComponent = (props: any) => {
   
    return <div>
    <div>{props.data.farmer.mobile }</div>
   
  </div>;
  };

  const LastVerficationComponent = (props: any) => {
   
    return <div>
   <div>
  {moment(props.data.farmer.mobileLastVerifiedOn).isValid()
    ? moment(props.data.farmer.mobileLastVerifiedOn).format(config.dateOnlyFormat)
    : ""}
</div>

   
  </div>;
  };
  const SourceComponent = (props: any) => {
   
    return <div>
     <div>{moment(props.data.farmer.mobileLastVerifiedOn).isValid() ? props.data.farmer.validationSource : ""}</div>
   
  </div>;
  };
  const [colDefs] = useState<any>([
    {
      field: "fullName",
      flex: 1,
      cellRenderer : FarmerComponent
    },
    { field: "systemId", flex: 1, headerName: "System Id" },
    { field: "beneficiaryId", flex: 1, headerName: "National Id" },
    {
      field: "mobile",
      flex: 1,
      cellRenderer : MobileComponent
    },

    { field: "isFarmerVerified", headerName: "Is Mobile Verified", flex: 1, cellRenderer: StatusComponent },
    {
      field: "mobileLastVerifiedOn", headerName: "Last Verified On", flex: 1,
      valueFormatter: function (params: any) {
        return params.value != null ? moment(params.value).format(config.dateOnlyFormat) : "";
      },
      cellRenderer : LastVerficationComponent
    },
    {
      field: "validationSource",
      flex: 1,
      cellRenderer : SourceComponent
    },
  ]);

  // const bindData = async () => {
  //   const searchParams = {};
  //   
  //   const response = await farmerService.getFarmerData(searchParams);
  //   
  //   setRowData(response.farmers);
  // }
  const bindData = async () => {
    const model = { statusId: -1, batchId: batchId };
    const response = await exportService.getDeductiblePayments(model);
    setRowData(response);
  };


  const bindImportHistory = async () => {
    try {
        const response = await importService.getImportSummaryByPaymentBatch(batchId);
        if (response && response.length > 0) {
          
            setImportHistory(response);
           
        }
    } catch (error) {
        console.error("Failed to bind import history:", error);
    }
};

    const afterConfirm = async (res: any) => {
      setShowImport(res)
      if (res == false) {
        setShowConfirmBox(false);
      }
      else {
        //navigate(`/payment-batch/review/${batch.id}`);
        //var result = await paymentBatchService.sendEmail(batch.id);
        const initValues = {
          action: '', // blank sets as Under Review
          remarks: ''
        };
        const response = await paymentBatchService.updatePaymentStage(initValues, batchId);
        setShowConfirmBox(false);
        toast.success('Sent for approval');
        setTimeout(() => {
          navigate(`/payment-batch/details/${batchId}`);

        }, 3000);
      }
    };

 const openConfirmBox = async () => {

    const confirmModel: IConfirmModel = {
      title: "Confirm action",
      btnText: "Send for Review",
      deleteUrl: ``,
      message: "confirm-stage-review",
    };

    setConfirmModel(confirmModel);
    setTimeout(() => {
      setShowConfirmBox(true);
    }, 500);
  };

  useEffect(() => {
    bindData();
    bindImportHistory();
  }, []);

  return (
    <Content>
      <InfoCard  onButtonClick= {openConfirmBox} importHistory={importHistory} setShowImport={setShowImport}/>
      <KTCard>

       {rowData && <CustomTable
          rowData={rowData}
          colDefs={colDefs}
        />} 
      </KTCard>
      {showConfirmBox && (
      <ConfirmBox
        confirmModel={confirmModel}
        afterConfirm={afterConfirm}
        loading={loading}
        btnType="theme"
      />
    )}
       
    </Content>
  
  )

}


export default PrePaymentKyc
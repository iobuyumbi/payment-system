import { useEffect, useRef, useState } from "react";

import { isAllowed } from "../../../../_metronic/helpers/ApiUtil";

import { PageLink, PageTitle } from "../../../../_metronic/layout/core";
import { KTCard, KTCardBody, KTIcon } from "../../../../_metronic/helpers";
import CustomTable from '../../../../_shared/CustomTable/Index'
import { Error401 } from "../../errors/components/Error401";
import { ConfirmBox } from "../../../../_shared/Modals/ConfirmBox";
import { IConfirmModel } from "../../../../_models/confirm-model";
import FarmerService from "../../../../services/FarmerService";
import AssociateService from "../../../../services/AssociateService";
import { toastNotify } from "../../../../services/NotifyService";
import {  useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
const farmerService = new FarmerService();
const associateService = new AssociateService();


const breadCrumbs: Array<PageLink> = [
    {
        title: 'Associates',
        path: '/associates',
        isSeparator: false,
        isActive: false,
    },
    {
        title: '',
        path: '',
        isSeparator: true,
        isActive: false,
    },
]
export default function AddAssociates(props: any) {
 

  const { setFarmers,afterConfirm , CountryId ,batchId} = props;
  const [loading, setLoading] = useState(false);
 
  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  
  const [rowData, setRowData] = useState<any>();
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
 

  const navigate = useNavigate();

  const [colDefs, setColDefs] = useState<any>([
    {
        field: 'select',
        width: 40,
        checkboxSelection: true,
        headerCheckboxSelection: true,
      },
    { field: "firstName", flex: 1, filter: true },
    { field: "otherNames", flex: 1, filter: true },
    { field: "country.countryName", flex: 1, headerName: 'Country', filter: true },
    { field: "mobile", flex: 1, filter: true },
    { field: "paymentPhoneNumber", flex: 1, headerName: 'Payment Phone', filter: true },
   
]);
const gridRef = useRef<any>();

const getSelectedRows = async () => {
  const toastId = toast.loading("Please wait...");
  const selectedRows = gridRef?.current.api.getSelectedRows();
  
  
  
  const farmersData = selectedRows.map((row: any) => ({
    farmerId: row.id, 
    paymentBatchId: batchId
  }));
  try {
    const response = await associateService.postAssociateData(farmersData);
  
    if (response && response[0].id) {
      navigate("/payment-batch");
      toastNotify(toastId, "Success");
    } else {
      toast.error("Something went wrong");
    }
  } catch (error) {
    toast.error("An error occurred");
  } finally {
    toast.dismiss(toastId);
  }
};

  

const bindFarmers = async () => {
    
        const data: any = {
            pageNumber: 1,
            pageSize: 10000,
            countryId : CountryId
        };
       
        const response = await farmerService.getFarmerData(data);
     
        setRowData(response);
    }
    useEffect(() => {
        bindFarmers();
    }, []);

  return (
    
       <div >
         <div className='card-toolbar d-flex justify-content-end m-3' data-kt-buttons='true'>
                        <button className='btn btn-sm btn-color-muted btn-active btn-active-primary active '
                            onClick={getSelectedRows}>
                             <KTIcon iconName='plus' className='fs-2' /> Save 
                        </button>

        </div>
        { isAllowed('payments.batch.history') ?  <> 
            <div className="modal-body ">
                    <CustomTable
                       rowData={rowData}
                       colDefs={colDefs}
                       header=""
                       gridRef={gridRef}
                       
                    />
               
            </div> </> : <Error401/>}
            {showConfirmBox && <ConfirmBox confirmModel={confirmModel} afterConfirm={afterConfirm} loading={loading} />}
            </div> 
    
  );
}

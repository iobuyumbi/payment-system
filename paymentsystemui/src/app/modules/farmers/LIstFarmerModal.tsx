import { Modal } from 'react-bootstrap'
import { useEffect, useState } from 'react';

import CustomTable from '../../../_shared/CustomTable/Index';
import FarmerService from '../../../services/FarmerService';

const farmerService = new FarmerService();

const FarmerListModal = ({ show, onSubmit, handleClose ,projectId}: { show: boolean, onSubmit: any, handleClose: any, projectId: any }) => {
    const [rowData, setRowData] = useState<any>([]);
    
    const onSelect = (props: any) => {
        onSubmit(props.data);
        handleClose();
    }
    
    const CustomActionComponent = (props: any) => {
        return <button className="btn btn-sm btn-default mx-0 p-1 link-primary fw-bold" onClick={() => onSelect(props)}>
            <i className="las la-link fs-2 text-primary "></i> Select farmer
        </button>;
    };

    const [colDefs] = useState<any>([
        { field: "Action", flex: 1, cellRenderer: CustomActionComponent },
        { field: "systemId", flex: 1, filter: true },
        { field: "firstName", flex: 1, filter: true },
        { field: "otherNames", flex: 1, filter: true },
        { field: "country.countryName", flex: 1, headerName: 'Country', filter: true },
        { field: "mobile", flex: 1, filter: true },
        { field: "paymentPhoneNumber", flex: 1, headerName: 'Payment Phone', filter: true },
        
    ]);

    const bindFarmers = async () => {
        
            const data: any = {
                pageNumber: 1,
                pageSize: 10000,
                projectId: projectId,
            };

            const response = await farmerService.getFarmerData(data);
            console.log("FarmerListModal response", response);
            setRowData(response.farmers);
        }
    

    useEffect(() => {
        
        bindFarmers();
    }, []);

    return (
        <Modal
            className='modal modal-xl modal-bottom-right'
            id='kt_Company_list'
            role='dialog'
            data-backdrop='false'
            tabIndex='-1'
            show={show}
            animation={true}
        >
            <div className='modal-content'>
                {/*begin::Header*/}
                <div className='d-flex align-items-center justify-content-between py-3 ps-8 pe-5 border-bottom'>
                    <h3 className='m-0'>Select Farmer</h3>
                    <div className='d-flex ms-2'>
                        {/*begin::Close*/}
                        <div
                            className='btn btn-icon btn-sm btn-light-primary ms-2'
                            data-bs-dismiss='modal'
                            onClick={handleClose}
                        >
                            <i className="las la-times-circle fs-2"></i>
                        </div>
                        {/*end::Close*/}
                    </div>
                </div>
                {/*end::Header*/}

                <CustomTable
                    rowData={rowData}
                    colDefs={colDefs}
                />
            </div>
        </Modal>
    )
}

export default FarmerListModal
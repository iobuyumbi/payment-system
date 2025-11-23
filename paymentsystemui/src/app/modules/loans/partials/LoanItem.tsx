
import { useState } from 'react'
import CustomTable from '../../../../_shared/CustomTable/Index';
import { KTIcon } from '../../../../_metronic/helpers';
import { IConfirmModel } from '../../../../_models/confirm-model';
import { ConfirmBox } from '../../../../_shared/Modals/ConfirmBox';
import { useNavigate } from 'react-router-dom';

const LoanItem = (props: any) => {
    const { loanItems , status} = props;
    const navigate = useNavigate();
    const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
    const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
    const [loading, setLoading] = useState(false);

    const afterConfirm = (res: any) => {
        navigate("/loans");
        setShowConfirmBox(false)
    }
    
    const openDeleteModal = (id: any) => {
        if (id) {
            const confirmModel: IConfirmModel = {
                title: 'Delete item',
                btnText: 'Delete this item?',
                deleteUrl: `api/loanItem/${id}`,
                message: 'delete-loanItem',
            }

            setConfirmModel(confirmModel)
            setTimeout(() => {
                setShowConfirmBox(true)
            }, 500)
        }
    }

    const CustomActionComponent = (props: any) => {
        
    
        return <div className="d-flex flex-row">
            {status !=="Disbursed" &&
            <button className="btn btn-default mx-0 px-1" onClick={() => openDeleteModal(props.data.id)}>
                <KTIcon iconName="trash" iconType="outline" className="text-danger fs-4" />
            </button>}
        </div>;
    };
    const itemArr = loanItems;
    const totalComponent = (props: any) => {
    
        const total = props.data.quantity * props.data.unitPrice;
        return <div>
            {total?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
        </div>;
    };

    const [colDefs, setColDefs] = useState<any>([
        { field: "itemName", flex: 1 },
        { field: "quantity", flex: 1 },
        { field: "unit", flex: 1 },
        { field: "unitPrice", flex: 1, valueFormatter: (params: any) => {
            
        return parseFloat(params.data.unitPrice)?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
      },  },
        { field: "total", flex: 1, cellRenderer: totalComponent },
        { field: "Action", flex: 1, cellRenderer: CustomActionComponent },
    ]);

    return (
        <div className='p-0 m-0'>
            <CustomTable
                rowData={itemArr}
                colDefs={colDefs}
                header=""
            />
            {showConfirmBox && <ConfirmBox confirmModel={confirmModel} afterConfirm={afterConfirm} loading={loading} />}
        </div>
    )
}

export { LoanItem }

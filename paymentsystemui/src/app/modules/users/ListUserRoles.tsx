import { useEffect, useState } from 'react';
import { Content } from '../../../_metronic/layout/components/content';
import { KTCard, KTIcon } from '../../../_metronic/helpers';
import { IConfirmModel } from '../../../_models/confirm-model';
import RoleService from '../../../services/RoleService';
import { useLocation } from 'react-router-dom';
import CustomTable from '../../../_shared/CustomTable/Index';
import { ConfirmBox } from '../../../_shared/Modals/ConfirmBox';

const roleService = new RoleService();

const ListUserRoles = () => {
    const location = useLocation();
    const state: any = location.state;

    const { id: userId, username } = state;

    // Row Data: The data to be displayed.
    const [rowData, setRowData] = useState([]);
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
    const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
    const [loading, setLoading] = useState(false);

    const editButtonHandler = (value: string) => {
    }

    const openDeleteModal = (id: any) => {
        if (id) {
            const confirmModel: IConfirmModel = {
                title: 'Delete role',
                btnText: 'Delete this role?',
                deleteUrl: `api/Roles/${id}`,
                message: 'delete-role',
            }

            setConfirmModel(confirmModel)
            setTimeout(() => {
                setShowConfirmBox(true)
            }, 500)
        }
    }

    const afterConfirm = (res: any) => {
        if (res) bindRoles()
        setShowConfirmBox(false)
    }

    const handleChange = (data: any) => {
    }
    
    const CustomActionComponent = (props: any) => {
        return <div className="d-flex flex-row">
            <button className="btn btn-default mx-0 px-1" onClick={() => editButtonHandler(props.data.departmentName)}>
                <KTIcon iconName="pencil" iconType="outline" className="fs-4" />
            </button>
            <button className="btn btn-default mx-0 px-1" onClick={() => openDeleteModal(props.data.id)}>
                <KTIcon iconName="trash" iconType="outline" className="text-danger fs-4" />
            </button>
        </div>;
    };

    const ManageActionComponent = (props: any) => {
        return <div onClick={() => handleChange(props.data)} style={{ cursor: 'pointer' }}>
            {props.data.selected ? <span className="badge badge-success">Y</span> : <span className="badge badge-danger">N</span>}
            {/* <Switch onChange={() => handleChange} checked={props.data.selected} /> */}
        </div>;
    };

    // Column Definitions: Defines the columns to be displayed.
    const [colDefs, setColDefs] = useState<any>([
        { headerName: "Role name", flex: 1, field: "roleName" },
        { headerName: "Status", flex: 1, field: "selected", cellRenderer: ManageActionComponent },
    ]);

    const bindRoles = async () => {
        //if (searchTerm.length === 0 || searchTerm.length > 2) {
        const response = await roleService.getRoles(userId);
        setRowData(response.userRoles);
        //}
    }

    useEffect(() => {
        bindRoles();
    }, [searchTerm]);

    return (
        <Content>
            <KTCard>
                <div className='card-header pt-5'>
                    <div className='card-title d-flex flex-column'>
                        <span className='fs-1 me-2'>
                            Manage roles for <span className='opacity-50'>{username} </span>  </span>
                        {/* <span className='opacity-75 pt-1 fw-semibold fs-6'>{'description'}</span> */}
                    </div>
                </div>

                <CustomTable
                    rowData={rowData}
                    colDefs={colDefs}
                    header="user-role"
                    addBtnText={"Add role"}
                    searchTerm={searchTerm}
                    setSearchTerm={setSearchTerm}
                    addBtnLink={"/roles/add"}
                    showSearchBox={false}
                />

            </KTCard>
            {showConfirmBox && <ConfirmBox confirmModel={confirmModel} afterConfirm={afterConfirm} loading={loading} />}
        </Content>
    )

}

export default ListUserRoles;
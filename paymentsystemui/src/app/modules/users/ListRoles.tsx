import { useEffect, useState } from 'react';
import { Content } from '../../../_metronic/layout/components/content';
import { KTCard, KTIcon } from '../../../_metronic/helpers';
import { IConfirmModel } from '../../../_models/confirm-model';
import { Link } from 'react-router-dom';
import RoleService from '../../../services/RoleService';
import CustomTable from '../../../_shared/CustomTable/Index';
import { ConfirmBox } from '../../../_shared/Modals/ConfirmBox';
import { isAllowed } from '../../../_metronic/helpers/ApiUtil';
import { Error401 } from '../errors/components/Error401';
import { filter } from 'lodash';

const roleService = new RoleService();

const ListRoles = () => {
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
                title: 'Delete user group',
                btnText: 'Delete this user group?',
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

    const CustomActionComponent = (props: any) => {
        return <div className="d-flex flex-row">
            {/* Edit */}
            {isAllowed('settings.system.users.groups.edit') &&
                <Link className="btn btn-default mx-0 px-1" to={`/account-settings/roles/edit/${props.data.id}`}>
                    <KTIcon iconName="pencil" iconType="outline" className="fs-4" />
                </Link>
            }

            {/* Delete */}
            {isAllowed('settings.system.users.groups.delete') &&
                <button className="btn btn-default mx-0 px-1" onClick={() => openDeleteModal(props.data.id)}>
                    <KTIcon iconName="trash" iconType="outline" className="text-danger fs-4" />
                </button>
            }
        </div>;
    };

    const ManageActionComponent = (props: any) => isAllowed('settings.system.roles.manage') && (<Link to={'/account-settings/role-permissions'} state={props.data} className='link-primary'>Manage Permissions</Link>)

    // Column Definitions: Defines the columns to be displayed.
    const [colDefs, setColDefs] = useState<any>([
        { headerName: "User Group name", flex: 1, field: "name", filter: true },
        { flex: 1, cellRenderer: ManageActionComponent },
        { flex: 1, cellRenderer: CustomActionComponent },
    ]);

    const bindRoles = async () => {
        if (searchTerm.length === 0 || searchTerm.length > 2) {
            const response = await roleService.getRoles(searchTerm);
            setRowData(response);
        }
    }

    useEffect(() => {
        bindRoles();
    }, [searchTerm]);

    return (
        <Content>
            {isAllowed("settings.system.users.groups.view") ? <KTCard>
                <CustomTable
                    rowData={rowData}
                    colDefs={colDefs}
                    header="role"
                    addBtnText={isAllowed('settings.system.users.groups.add') && "Add user group"}
                    searchTerm={searchTerm}
                    setSearchTerm={setSearchTerm}
                    addBtnLink={"/account-settings/roles/add"}
                    pageSize={20}
                />

            </KTCard> : <Error401 />}
            {showConfirmBox && <ConfirmBox confirmModel={confirmModel} afterConfirm={afterConfirm} loading={loading} />}
        </Content>
    )

}

export default ListRoles;
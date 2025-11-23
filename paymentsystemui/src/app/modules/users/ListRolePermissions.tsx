import { useEffect, useState } from 'react';
import { Content } from '../../../_metronic/layout/components/content';
import { KTCard } from '../../../_metronic/helpers';
import { IConfirmModel } from '../../../_models/confirm-model';
import { useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';
import PermissionService from '../../../services/PermissionService';
import CustomTable from '../../../_shared/CustomTable/Index';
import { ConfirmBox } from '../../../_shared/Modals/ConfirmBox';
import { isAllowed } from '../../../_metronic/helpers/ApiUtil';
import { Error401 } from '../errors/components/Error401';

const permissionService = new PermissionService();

const ListRolePermissions = () => {
    const location = useLocation();
    const state: any = location.state;
    const { id: roleId, name: roleName } = state;

    // Row Data: The data to be displayed.
    const [rowData, setRowData] = useState<any[]>([]);
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
    const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
    const [loading, setLoading] = useState(false);
    const [isAnyCheckboxChecked, setIsAnyCheckboxChecked] = useState(false);

    const afterConfirm = (res: any) => {
        if (res) bindRolePermissions()
        setShowConfirmBox(false)
    }

    const handleCheckboxChange = (params: any) => {
        setRowData(prevData =>
            prevData.map(row =>
                row.id === params.data.id
                    ? { ...row, selected: !row.selected }
                    : row
            )
        );

        setIsAnyCheckboxChecked(true);
        // Check if any checkbox is checked

        //     const updatedData = rowData.map((row) => {
        //         
        //         if (row.permissionName === params.data.permissionName) {
        //             return { ...row, selected: !row.selected };
        //         }
        //         return row;
        //     });
        //setRowData(updatedData);
    };

    const handleSave = async () => {
        // Handle save logic
        // alert(JSON.stringify(rowData));

        const filteredRowData = rowData.filter(row => row.selected)
        var response = await permissionService.saveRolePermission(roleId, filteredRowData);
        if (response.message) {
            toast(response.message);
            bindRolePermissions();
        }
    };

    // Column Definitions: Defines the columns to be displayed.
    const [colDefs, setColDefs] = useState<any[]>([
        {
            headerName: "Module", flex: 1, field: "moduleName", filter: true,

            rowSpan: (params: any) => {
                const moduleName = params.data.moduleName;
                const sameModuleRows = rowData.filter((row: any) => row.moduleName === moduleName);
                return sameModuleRows.length; // Span rows based on the number of similar 'moduleName'
            },
            cellRenderer: (params: any) => {
                // Use params.node.rowIndex to get the row index
                if (params.node.rowIndex === 0 || params.data.moduleName !== params.api.getDisplayedRowAtIndex(params.node.rowIndex - 1).data.moduleName) {
                    return (
                        <span className='fw-bold'>
                            {params.value}
                        </span>
                    );
                }
                return null;
            },
            suppressMovable: true,
        },
        { headerName: "Permission", flex: 1, field: "permissionDescription", filter: true },
        {
            headerName: 'Select',
            field: 'selected',
            width: 50,
            flex: 1,
            cellRenderer: (params: any) => (
                <div className="form-check">
                    <input
                        type="checkbox"
                        className="form-check-input mt-3 small-checkbox"
                        checked={params.value}
                        onChange={() => handleCheckboxChange(params)}
                    />
                </div>
            ),
        },
       
        //{ headerName: "Status", flex: 1, field: "selected", cellRenderer: ManageActionComponent },
    ]);

    const bindRolePermissions = async () => {
        const response = await permissionService.getRolePermissions(roleId);
        setRowData(response);
    }

    useEffect(() => {
        bindRolePermissions();
    }, [searchTerm]);

    return (
        <Content>
            {isAllowed('settings.system.users.groups.edit') ?
                <KTCard>
                    <div className='card-header pt-5'>
                        <div className='card-title d-flex flex-column'>
                            <span className='fs-1 me-2'>
                                Manage permissions for <span className='opacity-50'>{roleName} </span>  </span>
                            {/* <span className='opacity-75 pt-1 fw-semibold fs-6'>{'description'}</span> */}
                        </div>
                    </div>

                    <CustomTable
                        rowData={rowData}
                        colDefs={colDefs}
                        header="role-permissions"
                        addBtnText={"Save changes"}
                        searchTerm={searchTerm}
                        setSearchTerm={setSearchTerm}
                        addBtnLink={""}
                        showSearchBox={false}
                        showAddBtn={isAnyCheckboxChecked}
                        addBtnHandler={isAllowed('settings.system.users.groups.add') ? handleSave : ''}
                        height={1000}
                        pageSize={100}
                    />

                </KTCard> : <Error401 />}
            {showConfirmBox && <ConfirmBox confirmModel={confirmModel} afterConfirm={afterConfirm} loading={loading} />}
        </Content>
    )

}

export default ListRolePermissions;
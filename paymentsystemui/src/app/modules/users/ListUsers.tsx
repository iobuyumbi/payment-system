import { FC } from 'react'
import { useState, useEffect } from 'react';
import { Content } from '../../../_metronic/layout/components/content';
import { PageTitle, PageLink } from "../../../_metronic/layout/core";
import { PageTitleWrapper } from '../../../_metronic/layout/components/toolbar/page-title';
import CustomTable from '../../../_shared/CustomTable/Index'
import { KTCard } from '../../../_metronic/helpers';
import { IConfirmModel } from '../../../_models/confirm-model';
import { ConfirmBox } from '../../../_shared/Modals/ConfirmBox';
import { KTIcon } from '../../../_metronic/helpers';
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { addToUsers, resetUserState } from '../../../_features/users/userSlice';
import UserService from '../../../services/UserService';
import { isAllowed } from '../../../_metronic/helpers/ApiUtil';
import { Error401 } from '../errors/components/Error401';
import AuditListModal from '../../../_shared/Modals/AuditListModal';
import { requestPassword } from '../auth/core/_requests';
import { toast } from 'react-toastify';

const userService = new UserService();

const profileBreadCrumbs: Array<PageLink> = [
    {
        title: 'Users',
        path: '/account-settings/users',
        isSeparator: false,
        isActive: true,
    },
]
const ListUsers: FC = (props: any) => {
    const navigate = useNavigate();
    const dispatch = useDispatch();

    const [rowData, setRowData] = useState<any>();
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
    const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
    const [loading, setLoading] = useState(false);
    const [showAuditModal, setShowAuditModal] = useState(false);
    const [currentId, setCurrentId] = useState("");
      const [hasErrors, setHasErrors] = useState<boolean | undefined>(undefined);
  const [errors, setErrors] = useState<any>([]);

    const editButtonHandler = (value: any) => {
        console.log(value)
        dispatch(addToUsers(value));
        navigate(`/account-settings/users/edit/${value.id}`);
    }

    const openDeleteModal = (id: any) => {

        if (id) {
            const confirmModel: IConfirmModel = {
                title: 'Delete user',
                btnText: 'Delete this user?',
                deleteUrl: `api/user/${id}`,
                message: 'delete-user',
            }

            setConfirmModel(confirmModel)
            setTimeout(() => {
                setShowConfirmBox(true)
            }, 500)
        }
    }

    const afterConfirm = (res: any) => {
        if (res) {
            bindUsers()
        };
        setShowConfirmBox(false);
        setShowAuditModal(false);
    }

    const CustomActionComponent = (props: any) => {
        return <div className="d-flex flex-row">
            {/* Audit log */}
            {isAllowed('settings.system.users.view-audit') &&
                <button
                    onClick={() => {
                        setShowAuditModal(true);
                        setCurrentId(props.data.id);
                    }}
                    className="btn btn-default link-primary"
                >
                    Audit Log
                </button>
            }

            {/* Edit */}
            {isAllowed('settings.system.users.edit') &&
                <button className="btn btn-default mx-0 px-1" onClick={() => editButtonHandler(props.data)}>
                    <KTIcon iconName="pencil" iconType="outline" className="fs-4" />
                </button>
            }

            {/* Delete */}
            {isAllowed('settings.system.users.delete') &&
                <button className="btn btn-default mx-0 px-1" onClick={() => openDeleteModal(props.data.id)}>
                    <KTIcon iconName="trash" iconType="outline" className="text-danger fs-4" />
                </button>
            }
        </div>;
    };

const PasswordRestComponent = (props: any) => {
        return <div className="d-flex flex-row">
          

            {/* Edit */}
            {isAllowed('settings.system.users.edit') &&
                <button className="btn btn-default link-primary mx-0 px-1 " onClick={() => passwordResetHandler(props.data)}>
                  Send Email
                </button>
            }

           
        </div>;
    };

const passwordResetHandler = (value: any) => {
  setLoading(true);
  setErrors([]);
  setHasErrors(undefined);

  setTimeout(() => {
    const promise = requestPassword(value.email);

    toast.promise(promise, {
      pending: {
        render() {
          return "Requesting password reset...";
        },
        autoClose: false,
        closeButton: true,
      },
      success: {
        render() {
          setHasErrors(false);
          return "Password reset email sent!";
        },
        autoClose: 5000,
        closeButton: true,
      },
      error: {
        render({ data: err } : any) {
          const backendErrors =
            err?.response?.data?.errors ||
            err?.response?.data?.Errors ||
            ["Failed to reset password."];

          // Store errors in state for UI use
          setErrors(backendErrors);
          setHasErrors(true);

          // Combine all backend errors into one toast message
          return backendErrors.join(" ");
        },
        autoClose: 5000,
        closeButton: true,
      },
    })
      .finally(() => {
        setLoading(false);
      });
  }, 1000);
};

    // Column Definitions: Defines the columns to be displayed.
    const [colDefs, setColDefs] = useState<any>([
        { field: "username", flex: 1, filter: true },
        { field: "email", flex: 1.5, filter: true },
        { field: "phoneNumber", flex: 1, filter: true },
        { field: "roleName", headerName: 'User Group', flex: 1, filter: true },
        { field: "userCountriesStr", flex: 1, headerName: "Countries" },
        { field: "isLoginEnabled", flex: 1, filter: true },
        {field : "Reset Password", flex: 1, cellRenderer: PasswordRestComponent},
        { field: "Action", flex: 1, cellRenderer: CustomActionComponent },
    ]);

    const bindUsers = async () => {
        if (searchTerm.length === 0 || searchTerm.length > 2) {
            const response = await userService.getUserData();
            setRowData(response);
        }
    }

    useEffect(() => {
        bindUsers();
        dispatch(resetUserState());
    }, [searchTerm]);

    return (
        <>
            <Content>
                {isAllowed("settings.system.users.view") ? (<><PageTitleWrapper />
                    <PageTitle breadcrumbs={profileBreadCrumbs}> Users </PageTitle>

                    <KTCard className='shadow mt-10'>
                        <CustomTable
                            rowData={rowData}
                            colDefs={colDefs}
                            header="user"
                            addBtnText={isAllowed('settings.system.users.add') ? "Add user" : ''}
                            searchTerm={searchTerm}
                            setSearchTerm={setSearchTerm}
                            addBtnLink={isAllowed('settings.system.users.add') ? "/account-settings/users/add" : ''}
                            pageSize={20}
                            height={620}
                        />
                    </KTCard>
                </>) : (
                    <Error401 />
                )}
                {showConfirmBox && <ConfirmBox confirmModel={confirmModel} afterConfirm={afterConfirm} loading={loading} />}
                {showAuditModal && (
                    <AuditListModal
                        exModule={"User"}
                        componentId={currentId}
                        onClose={afterConfirm}
                    />
                )}
            </Content>
        </>
    );

}
export { ListUsers }

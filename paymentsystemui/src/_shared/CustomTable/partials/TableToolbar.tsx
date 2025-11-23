import { useNavigate } from "react-router-dom";
import { KTIcon } from "../../../_metronic/helpers";


const TableToolbar = (props: any) => {
  const { addBtnText, addBtnLink, addBtnHandler, handleExport, showExport = false } = props;
  const navigate = useNavigate();

  const addBtnClickHandler = () => {
    if (addBtnLink != "") {
      navigate(addBtnLink);
    } else {
      addBtnHandler()
    }
  }

  const importBtnClickHandler = () => {
    handleExport();
  }

  return (
    <div className='d-flex justify-content-end' data-kt-user-table-toolbar='base'>
      {/* <UsersListFilter /> */}

      {/* begin::Export */}
      {
        showExport && <button type='button' className='btn btn-light me-3' onClick={() => importBtnClickHandler()}>
          <KTIcon iconName='exit-up' className='fs-2' />
          Export
        </button>
      }
      {/* end::Export */}

      {/* begin::Add user */}
      {addBtnText != "" &&
        <button type='button' className='btn btn-theme' onClick={() => addBtnClickHandler()}>
          <KTIcon iconName='plus' className='fs-2' />
          {addBtnText}
        </button>
      }
      {/* end::Add user */}
    </div>
  )
}

export { TableToolbar }

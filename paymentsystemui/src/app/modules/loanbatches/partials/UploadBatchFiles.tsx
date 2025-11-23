import { useEffect, useState } from "react";
import FarmerService from "../../../../services/FarmerService";
import {
  RequiredMarker,
  ValidationField,
} from "../../../../_shared/components";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { useFormik } from "formik";
import * as Yup from "yup";
import {
  LoanApplicationModel,
  initLoanApplicationValues,
} from "../../../../_models/loan-application-model";
import { toast } from "react-toastify";
import { toastNotify } from "../../../../services/NotifyService";
import { useNavigate } from "react-router-dom";
import { PageLink } from "../../../../_metronic/layout/core";
import { useSelector } from "react-redux";
import LoanApplicationService from "../../../../services/LoanApplicationService";
import LoanBatchService from "../../../../services/LoanBatchService";
import { Table } from "react-bootstrap";
import DragAndDropFileUpload from "../../../../_shared/DragAndDropFileUpload/Index";
import { isAllowed } from "../../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../../errors/components/Error401";
import { Button } from "react-bootstrap";
import FarmerListModal from "../../farmers/LIstFarmerModal";
import { OptionType } from "../../../../_models/option-type";
import { after } from "lodash";

const farmerService = new FarmerService();
const applicationService = new LoanApplicationService();
const loanBatchService = new LoanBatchService();

const loanApplicationSchema = Yup.object().shape({
  witnessFullName: Yup.string()
    .required("Witness name is required")
    .max(50, "Cannot exceed 50 characters"),
  witnessRelation: Yup.string()
    .required("Witness relation is required")
    .max(100, "Cannot exceed 100 characters"),
  dateOfWitness: Yup.string().required("Date of witness is required"),
  witnessPhoneNo: Yup.string()
    .required("Witness phone number is required")
    .max(50, "Cannot exceed 12 characters"),
});

const breadCrumbs: Array<PageLink> = [
  {
    title: "Loan Aoolications",
    path: `/loan-applications`,
    isSeparator: false,
    isActive: true,
  },
];

const UploadBatchFiles = (props: any) => {
  const {  afterConfirm } = props;

  const loanBatch = useSelector((state: any) => state?.loanBatches);

  const [loading, setLoading] = useState(false);

  const [attachmentIds, setAttachmentIds] = useState<any>();

  

  const calculateInitialTotalPrice = (items: any[]) => {
    return items.reduce((acc, item) => acc + item.quantity * item.unitPrice, 0);
  };

  const [errorMessage, setErrorMessage] = useState("");
  const navigate = useNavigate();
  const [errors, setErrors] = useState<any>({});

  return (
    <>
      {isAllowed("loans.applications.add") ? (
        <div className="row mb-6 ">
          <div className="d-flex justify-content-between">
            <div>
              <DragAndDropFileUpload
                appId={loanBatch?.id}
                onUpload={(files: any) => {setAttachmentIds(files)
                    afterConfirm(false)
                window.location.reload();}
                }
                isBatchFile={true}
              />
              <ul className="fs-7 ">
                <li>Max size : 3MB</li>
                <li>Types supported : PNG,JPG, and PDF</li>
              </ul>
            </div>
           
          </div>

          <div className="card-footer d-flex justify-content-center py-6 px-9">
            {/* <button type="submit" className="btn btn-theme">
              <span className="indicator-label">Submit</span>
            </button> */}
            <button
              type="button"
               onClick={() => afterConfirm(false)}
              className=" btn btn-light mx-3"
            >
              Cancel
            </button>
          </div>
        </div>
      ) : (
        <Error401 />
      )}
    </>
  );
};

export default UploadBatchFiles;

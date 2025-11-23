import { ErrorMessage, Field, Form, Formik } from "formik";
import { useState } from "react";
import { reviewValidationSchema } from "../../../../_shared/validations";
import { isAllowed } from "../../../../_metronic/helpers/ApiUtil";

const StartReviewCard = ({ onStartReview, loading ,authorisedUser}: any) => {
  const initValues = {
    action: "",
    remarks: "",
  };
  const [initialValues, setInitialValues] = useState(initValues);

  return (
    <div>
      {isAllowed("payments.batch.review") ? (
        <div className="row">
          <div className="col-md-12">
            <Formik
              initialValues={initialValues}
              enableReinitialize={true}
              validationSchema={reviewValidationSchema}
              onSubmit={onStartReview}
            >
              {({ values, isSubmitting, setFieldValue, ...formikProps }) => (
                <Form placeholder={undefined}>
                  <div className="row mb-5">
                    <div className="col-md-6 form-group">
                      <label className="form-label fw-bolder text-gray-800 fs-6 mb-1">
                        Review status <span className="text-danger">*</span>
                      </label>
                      <Field
                        as="select"
                        name="action"
                        className="block w-full form-control"
                      >
                        <option value="" label="Select" />
                        <option
                          value="review_rejected"
                          label="Review Rejected"
                        />
                        <option value="review_next" label="Send for Approval" />
                      </Field>
                      <ErrorMessage
                        name="action"
                        component="div"
                        className="text-danger mt-2"
                      />
                    </div>
                  </div>
                  <div className="row mb-5">
                    <div className="col-md-6">
                      <label className="form-label fw-bolder text-gray-800 fs-6 mb-1">
                        Remarks
                      </label>
                      <Field
                        as="textarea"
                        name="remarks"
                        className="form-control form-control-lg"
                        rows={3}
                      ></Field>
                      <ErrorMessage
                        name="remarks"
                        component="div"
                        className="text-danger mt-2"
                      />
                    </div>
                  </div>
                  <div className="text-gray-700">
                    <button type="submit" className="btn btn-primary">
                      {loading ? "Please wait..." : "Submit"}
                    </button>
                  </div>
                </Form>
              )}
            </Formik>
          </div>
        </div>
      ) : (
        <div className="bg-gray fw-bold text-center">
          You do not have permission to review payments.
           {onStartReview &&
        authorisedUser?.length > 0 && (
          <>
           <div className="bg-gray fw-bold text-center">
             Only people listed below can review or approve the payment batch.
            </div>
            <div className="text-center mt-2">
              {authorisedUser?.map((user: any, index: any) => (
                <span key={user.id}>
                  {user.username}
                  {index < authorisedUser?.length - 1 && ", "}
                </span>
              ))}
            </div>
          </>
        )}
        </div>



      )}
    </div>
  );
};

export default StartReviewCard;

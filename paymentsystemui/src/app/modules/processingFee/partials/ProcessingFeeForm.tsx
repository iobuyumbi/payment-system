import React, { useEffect } from 'react';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import { useState } from 'react';
import { RequiredMarker } from '../../../../_shared/components';
import { Link, useNavigate, useParams } from 'react-router-dom';
import ProcessingFeeService from '../../../../services/ProcessingFeeService';


// Define the shape of form values
interface LoanProcessingFeeFormValues {
  feeName: string;
  feeType: string;
  value?: number;
}

// Initial form values
const initialValues: any = {
  feeName: '',
  feeType: '',
  value: 0,
};

// Validation schema
const validationSchema = Yup.object().shape({
  feeName: Yup.string()
    .required('Fee name is required')
    .max(100, 'Max 100 characters'),

  feeType: Yup.string()
    .required('Fee type is required'),

  value: Yup.number()
    .typeError('Value must be a number')
    .when('feeType', {
      is: 'percent',
      then: (schema) =>
        schema
          .required('Percentage value is required')
          .moreThan(0, 'Percentage must be greater than 0')
          .max(100, 'Percentage cannot exceed 100'),
      otherwise: (schema) =>
        schema
          .required('Flat amount is required')
          .moreThan(0, 'Amount must be greater than 0')
          .max(999999, 'Amount cannot exceed 999,999'),
    }),
});


const processingFeeService = new ProcessingFeeService();

const ProcessingFeeForm: React.FC = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [data, setData] = useState<any>(initialValues);
  const [submissionMessage, setSubmissionMessage] = useState<string | null>(null);

  const handleSubmit = async (values: LoanProcessingFeeFormValues) => {
    const response = id ? await processingFeeService.updateProcessingFee(id, values) : await processingFeeService.addProcessingFee(values);
    if (response.id) {
      setSubmissionMessage('Form submitted successfully!');
      navigate('/processing-fee');
    }
  };
  const bindFee = async () => {
    const response = await processingFeeService.getProcessingFee();
    const filtered = response.filter((item: any) => item.id === id);
    setData(filtered[0]);
  };

  useEffect(() => {

    document.title = `Manage Processing Fee - SDD`;
    bindFee();
  }, [id]);


  return (
    <div className="container mx-auto p-6">
      <RequiredMarker />

      <Formik
        initialValues={data || initialValues}
        enableReinitialize={true}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
      >
        {({ isSubmitting, ...formiklProps }) => (
          <Form className="space-y-4" placeholder={'processing_fee_form'}>
            <div className="row mb-5">
              <div className="col-md-6">
                <div>
                  <label htmlFor="feeName" className="fw-bold fs-6 mb-2">Fee name</label>
                  <Field
                    type="text"
                    name="feeName"
                    className="block w-full form-control"
                  />

                  <div className='fv-plugins-message-container'>
                    <div className='fv-help-block'>
                      <span role='alert'><ErrorMessage name="feeName" component="div" className="py-1" /></span>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div className="row mb-5">
              <div className="col-md-6">
                <label className="fw-bold fs-6 mb-2 me-3">Fee Type</label>
                <Field as="select" name="feeType" className="block w-full form-control">
                  <option value="" label="Select a fee" />
                  <option value="flat" label="Flat Amount" />
                  <option value="percent" label="Percentage" />
                </Field>
                <div className='fv-plugins-message-container'>
                  <div className='fv-help-block'>
                    <span role='alert'><ErrorMessage name="feeType" component="div" className="py-1" /></span>
                  </div>
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col-md-6">
                <label htmlFor="value" className="fw-bold fs-6 mb-2">Value</label>
                <Field
                  type="number"
                  name="value"
                  className="block w-full form-control"
                />

                <div className='fv-plugins-message-container'>
                  <div className='fv-help-block'>
                    <span role='alert'><ErrorMessage name="value" component="div" className="py-1" /></span>
                  </div>
                </div>
              </div>
            </div>
            {/* <div className="row mb-5">
              <div className="col-md-6">
                {values.isPercentageBased ? (
                  <div>
                    <label htmlFor="percentage" className="fw-bold fs-6 mb-2">Percentage (%)</label>
                    <Field
                      type="number"
                      name="percentage"
                      className="block w-full form-control"
                    />

                    <div className='fv-plugins-message-container'>
                      <div className='fv-help-block'>
                        <span role='alert'><ErrorMessage name="percentage" component="div" className="py-1" /></span>
                      </div>
                    </div>
                  </div>
                ) : (
                  <div>
                    <label htmlFor="flatAmount" className="fw-bold fs-6 mb-2">Flat Amount</label>
                    <Field
                      type="number"
                      name="flatAmount"
                      className="block w-full form-control"
                    />

                    <div className='fv-plugins-message-container'>
                      <div className='fv-help-block'>
                        <span role='alert'><ErrorMessage name="flatAmount" component="div" className="py-1" /></span>
                      </div>
                    </div>
                  </div>
                )}
              </div>
            </div> */}

            <div className="d-flex justify-content-center py-6 px-9">
              <button
                type="submit"
                disabled={isSubmitting}
                className="btn btn-theme"
              >
                {isSubmitting ? 'Submitting...' : 'Submit'}
              </button>
              <Link to={"/processing-fee"} className=" btn btn-light mx-3">
                Cancel
              </Link>
            </div>
          </Form>
        )}
      </Formik>

      {submissionMessage && (
        <p className="text-primary-500 mt-4">{submissionMessage}</p>
      )}
    </div>
  );
};

export default ProcessingFeeForm;


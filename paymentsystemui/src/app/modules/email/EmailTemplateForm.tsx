import { Formik, Form, Field, ErrorMessage } from 'formik';
import { Editor } from '@tinymce/tinymce-react';
import { toast } from 'react-toastify';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { emailValidationSchema } from '../../../_shared/validations';
import EmailService from '../../../services/EmailService';
import SubmitButton from '../../../_shared/SubmitButton/Index';

const emailService = new EmailService();

const EmailTemplateForm = () => {
    const { id }: any = useParams();
    const navigate = useNavigate();

    const initValues = {
        name: '',
        subject: '',
        body: '',
    };
    const [initialValues, setInitialValues] = useState(initValues);

    const onSubmit = async (values: any, { setSubmitting }: any) => {
        const variables: { name: any; defaultValue: string; }[] = [];
        const variableRegex = /\{([^{}]+)\}/g; // Regular expression to match variables surrounded by curly braces
        values.body.replace(variableRegex, (match: any, variableName: any) => {
            variables.push({ name: variableName, defaultValue: '' }); // Extract the variable name and add it to the list
        });
        values.name = values.name;
        values.variables = variables;

        if (id) {
            const response = await emailService.updateEmailTemplate(id, values);
            if (response && response.id) {
                toast.success('Saved successfully');
                navigate('/email/templates')
            } else {
                toast.error('Something went wrong!');
            }
        } else {
            const response = await emailService.saveEmailTemplate(values);
            if (response && response.id) {
                toast.success('Saved successfully');
            } else {
                toast.error('Something went wrong!');
            }
        }

        setSubmitting(false);
    };

    const bindDetail = async () => {
        const response = await emailService.getTemplateByName(id);
        if (response) {
            setInitialValues({
                name: response.name,
                subject: response.subject,
                body: response.body,
            });
        }
    }

    useEffect(() => {
        if (id) {
            bindDetail();
        }
    }, [id]);

    return (
        <div>
            <Formik
                initialValues={initialValues}
                enableReinitialize={true}
                validationSchema={emailValidationSchema}
                onSubmit={onSubmit}
            >
                {({ values, isSubmitting, setFieldValue, ...formikProps }) => (
                    <Form placeholder={undefined}>
                        <div className="row mb-5">
                            <div className="col-md-6 form-group">
                                <label className="form-label fw-bolder text-gray-800 fs-6 mb-1">Template Name <span className='text-danger'>*</span></label>
                                <Field type="text" name="name" className="form-control form-control-solid" readOnly/>
                                <ErrorMessage name="name" component="div" className="error mt-2" />
                            </div>
                        </div>
                        <div className="row mb-5">
                            <div className="col-md-6">
                                <label className="form-label fw-bolder text-gray-800 fs-6 mb-1">Subject <span className='text-danger'>*</span></label>
                                <Field type="text" name="subject" className="form-control" />
                                <ErrorMessage name="subject" component="div" className="error mt-2" />
                            </div>
                        </div>
                        <div className="row mb-5">
                            <div className="col-md-12">
                                <label className="form-label fw-bolder text-gray-800 fs-6 mb-1">Body <span className='text-danger'>*</span></label>
                                <Editor
                                    id="body"
                                    apiKey='y18oqxlui4dyi52x9tzx5b9099wb5ypmg92u2rvfq12ch76k'
                                    initialValue={values.body} // Set initial value
                                    init={{
                                        height: 450,
                                        menubar: true,
                                        plugins: [
                                            'advlist autolink lists link image charmap print preview anchor',
                                            'searchreplace visualblocks code fullscreen',
                                            'insertdatetime media table paste code help wordcount',
                                        ],
                                        toolbar:
                                            'undo redo | formatselect | bold italic backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | removeformat | help',
                                    }}
                                    onEditorChange={(newValue: any) => {
                                        setFieldValue('body', newValue);
                                    }}
                                />
                                <ErrorMessage name="body" component="div" className="error mt-2" />
                            </div>
                        </div>

                        <div className="row mb-5">
                            <div className="col-md-12">
                                {/* <button className="btn btn-primary" type="submit" disabled={isSubmitting}>
                                    {isSubmitting ? 'Submitting...' : 'Save Template'}
                                </button> */}
                                <SubmitButton className="me-3" loading={isSubmitting} formik={formikProps}>Save Template</SubmitButton>
                                <Link
                                    to={"/email/templates"}
                                    className="btn btn-secondary"
                                    type="button"
                                >
                                    Cancel
                                </Link>
                            </div>
                        </div>
                    </Form>
                )}
            </Formik>
        </div>
    );
};

export default EmailTemplateForm;
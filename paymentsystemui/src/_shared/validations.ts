import * as Yup from 'yup'

export const emailValidationSchema = Yup.object().shape({
  // name: Yup.string()
  //     .required('Template name is required')
  //     .min(2, 'Template name must be at least 2 characters')
  //     .max(50, 'Template name must not exceed 50 characters'),
  subject: Yup.string()
    .required('Subject is required')
    .min(2, 'Subject must be at least 2 characters')
    .max(250, 'Subject must not exceed 250 characters'),
  body: Yup.string()
    .required('Body is required')
    .min(2, 'Body must be at least 2 characters'),
});

export const reviewValidationSchema = Yup.object().shape({
  action: Yup.string()
    .required('Select review status'),
  remarks: Yup.string()
    .nullable()
    .when('action', {
      is: "review_rejected",
      then: (schema) => schema.required('Remarks are required'),
      otherwise: (schema) => schema.nullable(),
    }),
});
// import React from 'react'

// const loanbatcfform_old = () => {
//   return (
//     <div>

//  <form noValidate className="form" onSubmit={formik.handleSubmit}>
//             <div className="card mt-10 mb-5 mb-xl-10 shadow">
//               <div className="card-body p-9">
//                 <RequiredMarker />

//                 <div className="row mb-4">
//                   <ValidationField
//                     className="col-md-5"
//                     label="Name"
//                     isRequired
//                     name="name"
//                     type="text"
//                     placeholder="Enter name"
//                     formik={formik}
//                   />
//                   <div className="col-md-5 offset-md-1">
//                     {/* <CountryDropdown formik={formik} isRequired={true} /> */}
//                     <ProjectDropdown
//                       formik={formik}
//                       isRequired={true}
//                       isDisabled={isDisabled}
//                     />
//                   </div>
//                 </div>

//                 <div className="row mb-4 ">
//                   <ValidationSelect
//                     className="col-md-5"
//                     label="Status"
//                     name="statusId"
//                     defaultValue={data.statusId}
//                     options={[
//                       { id: 0, name: "Select status" },
//                       { id: 1, name: "Open" },
//                       { id: 2, name: "In Review" },
//                       { id: 3, name: "Accepting Applications" },
//                       { id: 4, name: "Closed" },
//                     ]}
//                     formik={formik}
//                   />
//                   <ValidationField
//                     className="col-md-5 offset-md-1"
//                     label="Interest rate"
//                     isRequired
//                     name="interestRate"
//                     type="number"
//                     placeholder="Enter interest rate"
//                     formik={formik}
//                   />
//                 </div>
//                 <div className="row mb-4 ">
//                   <ValidationSelect
//                     className="col-md-5"
//                     label="Intrest rate type"
//                     name="rateType"
//                     defaultValue={data.rateType}
//                     options={[
//                       { id: "", name: "Select status" },
//                       { id: "Flat", name: "Flat rate" },
//                       { id: "Reducing", name: "Reducing balance" },

//                     ]}
//                     formik={formik}
//                   />
//                   <ValidationSelect
//                     className="col-md-5 offset-md-1"
//                     label="Intrest is calculated"
//                     name="calculationTimeframe"
//                     defaultValue={data.calculationTimeframe}
//                     options={[
//                       { id: "", name: "Select status" },
//                       { id: "Yearly", name: "Yearly" },
//                       { id: "Monthly", name: "Monthly" },

//                     ]}
//                     formik={formik}
//                   />
//                 </div>

//                 <div className="row mb-4 ">

//                   <ValidationField
//                     className="col-md-5 "
//                     label="Tenure (in month)"
//                     isRequired
//                     name="tenure"
//                     type="number"
//                     placeholder="Enter tenure"
//                     formik={formik}
//                   />
//                   <ValidationField
//                     className="col-md-5 offset-md-1"
//                     label="Grace period (in months)"
//                     isRequired
//                     name="gracePeriod"
//                     type="number"
//                     placeholder="Enter grace periods"
//                     formik={formik}
//                   />
//                 </div>
//                 <div className="row mb-4 ">

//                   <ValidationField
//                     className="col-md-5 "
//                     label="Processing fees"
//                     isRequired
//                     name="processingFee"
//                     type="number"
//                     placeholder="Enter processing fee"
//                     formik={formik}
//                   />
//                   <div className="col-md-5 offset-md-1">
//                     <div className="row">
//                       <div className="col-md-5">
//                         <label className="fs-6 fw-bold">Effective Date</label>
//                       </div>
//                       <div className="col-md-12 mt-2">
//                         <DatePicker
//                           selected={formik.values.effectiveDate}
//                           className="form-control"
//                           name="effectiveDate"
//                           onChange={(date) =>
//                             formik.setFieldValue("effectiveDate", date)
//                           }
//                         />
//                       </div>
//                     </div>
//                   </div>
//                 </div>


//                 <div className="row mb-4">
//                   <div className="col-md-5 mb-5">
//                     <div className="row">
//                       <div className="col-md-5">
//                         <label className="fs-6 fw-bold">Initiation Date</label>
//                       </div>
//                       <div className="col-md-12 mt-2">
//                         <DatePicker
//                           selected={formik.values.initiationDate}
//                           className="form-control"
//                           name="initiationDate"
//                           onChange={(date) =>
//                             formik.setFieldValue("initiationDate", date)
//                           }
//                         />
//                       </div>
//                     </div>
//                   </div>
//                   <div className="col-md-11 ">
//                     <ValidationTextArea
//                       className="col-md-12"
//                       label="Payment terms"
//                       isRequired
//                       name="paymentTerms"
//                       type="text"
//                       rows={6}
//                       placeholder="Enter payment terms"
//                       formik={formik}
//                     />
//                   </div>

//                 </div>
//                 <div className="row">
//                   <div className="col-md-6">
//                     <FieldArray name="processingFees">
//                       {({ remove, push }) => (
//                         <div>
//                           {data.processingFees.map((_: any, index: number) => (
//                             <div key={index} className="border p-4 mb-4 rounded">
//                               <div className="flex space-x-4 items-center">
//                                 <div className="flex-1">
//                                   <label htmlFor={`processingFees.${index}.feeName`}>Fee Type</label>
//                                   <Field
//                                     as="select"
//                                     name={`processingFees.${index}.feeName`}
//                                     className="block border p-2 rounded w-full"
//                                     formik={formik}
//                                   >
//                                     <option value="">Select Fee</option>
//                                     {feeOptions.map((option) => (
//                                       <option key={option.id} value={option.id}>
//                                         {option.feeName}
//                                       </option>
//                                     ))}
//                                   </Field>
//                                   <ErrorMessage
//                                     name={`processingFees.${index}.feeName`}
//                                     component="div"
//                                     className="text-red-500"
//                                   />
//                                 </div>

//                                 <div className="flex-1">
//                                   <label htmlFor={`processingFees.${index}.amount`}>Amount</label>
//                                   <Field
//                                     type="number"
//                                     name={`processingFees.${index}.amount`}
//                                     className="block border p-2 rounded w-full"
//                                   />
//                                   <ErrorMessage
//                                     name={`processingFees.${index}.amount`}
//                                     component="div"
//                                     className="text-red-500"
//                                   />
//                                 </div>

//                                 <button
//                                   type="button"
//                                   onClick={() => remove(index)}
//                                   className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600"
//                                 >
//                                   Remove
//                                 </button>
//                               </div>
//                             </div>
//                           ))}

//                           <button
//                             type="button"
//                             onClick={() => push({ feeType: '', amount: undefined })}
//                             className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
//                           >
//                             Add Processing Fee
//                           </button>
//                         </div>
//                       )}
//                     </FieldArray>

//                   </div>
//                 </div>
//                 <div className="card-footer d-flex justify-content-center py-6 px-9">
//                   <button
//                     type="submit"
//                     className="btn btn-theme"
//                   // disabled={loading || formik.isSubmitting || !formik.isValid || !formik.touched}
//                   >
//                     <span className="indicator-label">Submit</span>
//                     {formik.isSubmitting && (
//                       <span className="indicator-progress">
//                         Please wait...{" "}
//                         <span className="spinner-border spinner-border-sm align-middle ms-2"></span>
//                       </span>
//                     )}
//                   </button>
//                   <Link to={"/loans/batches"} className=" btn btn-light mx-3">
//                     Cancel
//                   </Link>
//                 </div>
//               </div>
//             </div>
//           </form>

//     </div>
//   )
// }

// export default loanbatcfform_old
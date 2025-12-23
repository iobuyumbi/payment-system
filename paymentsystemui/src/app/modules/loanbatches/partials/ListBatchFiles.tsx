import React, { useEffect, useState } from "react";
import { toAbsoluteUrl } from "../../../../_metronic/helpers"; // Adjust the path as needed
import { getAPIBaseUrl } from "../../../../_metronic/helpers/ApiUtil";
import { round } from "lodash";
import { Content } from "../../../../_metronic/layout/components/content";
import BatchFileUploadModal from "./BatchFileUploadModal";
import LoanBatchService from "../../../../services/LoanBatchService";
import { useParams } from "react-router-dom";

const loanBatchService = new LoanBatchService(); // Ensure this service is imported correctly
const ListBatchFiles = (props: any) => {
  const { batchId } = props;
  const { id } = useParams();
  const API_URL = getAPIBaseUrl();
  const [showItemBox, setShowItemBox] = useState<boolean>(false);
  const [attachments, setAttachments] = useState<any>({});
  const afterConfirm = (value: any) => {
    setShowItemBox(value);
  };

  const bindFiles = async () => {
    const files = await loanBatchService.getLoanBatchFiles(batchId);
    if (files && files.length > 0) {
      // handle files here
      setAttachments(files);
    }
  };

  const downloadHandler = async (filePath: any) => {
    try {
      const baseUrl = getAPIBaseUrl();
      const completeUrl = baseUrl + "wwwroot/" + filePath;
      const response = await fetch(completeUrl);

      // Check if the request was successful
      if (!response.ok) {
        throw new Error("Failed to fetch file");
      }

      // Convert the response to a Blob
      const blob = await response.blob();

      // Create a URL for the Blob
      const url = window.URL.createObjectURL(blob);

      // Create a temporary anchor element
      const a = document.createElement("a");
      a.href = url;
      a.download = filePath.split("/").pop();
      document.body.appendChild(a);
      a.click();

      // Clean up
      document.body.removeChild(a);
      window.URL.revokeObjectURL(url);
    } catch (error) {
      console.error("Error downloading file:", error);
    }
  };

  useEffect(() => {
    bindFiles();
  }, [batchId]);

  return (
    <Content>
      <div className="card m-2">
        <div className="card-header border-0 py-5">
          <h3 className="card-title align-items-start flex-column">
            <span className="card-label fw-bold fs-4 mb-1">
              Loan Product Files
            </span>
            {/* <span className='text-muted fw-semibold fs-7'>More than 500 new orders</span> */}
          </h3>

          {/* begin::Toolbar */}
          <div className="card-toolbar flex" data-kt-buttons="true">
            <button
              type="button"
              className="btn btn-sm btn-color-muted btn-active btn-active-primary active px-4 me-3 mx-2 w-100px"
              onClick={() => setShowItemBox(true)}
            >
              Upload File
            </button>
          </div>
          {/* end::Toolbar */}
        </div>

        <div className="card-body">
          <div className="timeline-item">
            <div className="timeline-line w-40px"></div>

            <div className="timeline-content mb-10 mt-n1">
              <div className="overflow-auto pb-5">
                <div className="d-flex align-items-center border border-dashed border-gray-300 overflow-auto rounded min-w-700px p-5 m-5">
                  {Array.isArray(attachments) && attachments.length > 0 && (
                    <div className="d-flex flex-aligns-center flex-wrap">
                      {attachments.map((item: any) => {
                        const getFileIcon = (contentType: string): string => {
                          if (contentType === "application/pdf") {
                            return "media/svg/files/pdf.svg";
                          } else if (
                            ["image/png", "image/jpeg"].includes(contentType)
                          ) {
                            return "media/svg/files/image.svg"; // Replace with your actual image icon path
                          }
                          return "media/svg/files/blank-image.svg";
                        };

                        const fileSizeInMB = item?.fileSize
                          ? (item.fileSize / (1024 * 1024)).toFixed(2)
                          : "0.00";

                        return (
                          <div
                            className="d-flex flex-aligns-center pe-10 pe-lg-20 mb-3"
                            key={item?.fileName}
                            onClick={() => downloadHandler(item?.imagePath)}
                            role="button"
                          >
                            <img
                              alt=""
                              className="w-30px me-3"
                              src={toAbsoluteUrl(
                                getFileIcon(item?.contentType)
                              )}
                            />
                            <a
                              href={`${API_URL}uploads/loanbatchfiles/${item?.fileName}`}
                              target="_blank"
                              rel="noopener noreferrer"
                            >
                              <div className="md-1 fw-bold">
                                {item?.fileName}
                                <div className="text-gray-500">
                                  Size: {fileSizeInMB} MB
                                </div>
                              </div>
                            </a>
                          </div>
                        );
                      })}
                    </div>
                  )}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      {showItemBox && (
        <BatchFileUploadModal afterConfirm={afterConfirm} isAdd={true} />
      )}
    </Content>
  );
};

export default ListBatchFiles;

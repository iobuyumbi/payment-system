import { useState } from 'react';
import axios from 'axios';
import { AUTH_LOCAL_STORAGE_KEY } from '../../app/modules/auth';
import { getAPIBaseUrl } from '../../_metronic/helpers/ApiUtil';

const axiosInstance = axios.create();

const DragAndDropFileUpload = ({ onUpload, appId, url, allowedTypes, isBatchFile = false }: any) => {
    const [dragActive, setDragActive] = useState(false);
    const [files, setFiles] = useState<any>([]);
    const maxFiles = 5;
    const maxSize = 2 * 1024 * 1024; // 2MB in bytes
    allowedTypes = allowedTypes ? allowedTypes : ['image/jpeg', 'image/png', 'application/pdf']; // Allowed MIME types

    const handleDrag = (event: any) => {
        event.preventDefault();
        event.stopPropagation();
        if (event.type === 'dragenter' || event.type === 'dragover') {
            setDragActive(true);
        } else if (event.type === 'dragleave') {
            setDragActive(false);
        }
    };

    const handleDrop = (event: any) => {
        event.preventDefault();
        event.stopPropagation();
        setDragActive(false);
        if (event.dataTransfer.files && event.dataTransfer.files.length > 0) {
            const selectedFiles = Array.from(event.dataTransfer.files);
            setFiles(selectedFiles);
            onUpload(selectedFiles);
            event.dataTransfer.clearData();
        }
    };

    const handleChange = async (event: any) => {
        const selectedFiles = Array.from(event.target.files);

        // Validate file count
        if (selectedFiles.length + files.length > maxFiles) {
            alert(`You can only upload up to ${maxFiles} files.`);
            return;
        }

        // Validate each file
        const validatedFiles: any = selectedFiles.filter((file: any) => {
            if (!allowedTypes.includes(file.type)) {
                alert(`${file.name} is not a supported file type.`);
                return false;
            }
            if (file.size > maxSize) {
                alert(`${file.name} exceeds the maximum file size of 2MB.`);
                return false;
            }
            return true;
        });

        // Update state with valid files
        setFiles([...files, ...validatedFiles]);


        let formData: FormData = new FormData();

        for (var j = 0; j < validatedFiles.length; j++) {
            formData.append("file", validatedFiles[j], validatedFiles[j].name);
        }
        var _url = url ? url : (isBatchFile ? `api/FileUpload/LoanBatchUpload?appId=${appId}` : `api/FileUpload/loanapplications?appId=${appId}`);
        var upload_response = await uploadFile(formData, _url);

        onUpload(upload_response.data.data);
    };

    const handleRemoveFile = (index: any) => {
        const updatedFiles = files.filter((_: any, i: any) => i !== index);
        setFiles(updatedFiles);
        onUpload(updatedFiles);
    };

    const uploadFile = async (formData: any, url: any) => {
        let token = '';
        const aware_user = JSON.parse(localStorage.getItem(AUTH_LOCAL_STORAGE_KEY) || "");
        if (aware_user) {
            token = aware_user?.api_token;
        }
        const response = axiosInstance.post(url, formData,
            {
                baseURL: getAPIBaseUrl(),
                headers: {
                    'Content-Type': 'multipart/form-data',
                    'Access-Control-Allow-Origin': '*',
                    'Access-Control-Allow-Methods': 'POST, GET, OPTIONS',
                    Authorization: 'Bearer ' + token,
                },
            });

        return response;
    }

    return (
        <div>
            <input
                id="fileInput"
                type="file"
                multiple
                onChange={handleChange}
                style={{ display: 'none' }}
            />
            <label
                htmlFor="fileInput"
                className={`drag-drop-zone ${dragActive ? 'active' : ''}`}
                onDragEnter={handleDrag}
                onDragLeave={handleDrag}
                onDragOver={handleDrag}
                onDrop={handleDrop}
                style={{
                    border: '2px dashed #cccccc',
                    borderRadius: '10px',
                    padding: '20px',
                    textAlign: 'center',
                    cursor: 'pointer',
                    color: '#666666',
                    backgroundColor: dragActive ? '#f0f8ff' : '#ffffff',
                }}
            >
                <p>Drag and drop files here or click to select files</p>
                <ul>
                    {files.map((file: any, index: any) => (
                        <li key={index} className="mb-3">
                            {file.name}
                            <button type="button" className="btn btn-sm sdd-gray-bg" onClick={() => handleRemoveFile(index)} style={{ marginLeft: '10px' }}>
                                Remove
                            </button>
                        </li>
                    ))}
                </ul>
            </label>
        </div>
    );
};

export default DragAndDropFileUpload;

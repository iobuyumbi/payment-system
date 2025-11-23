/* eslint-disable prefer-const */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { useState } from 'react';
import { Form } from 'react-bootstrap';
const UploadLayout = (props: any) => {
    const { setFiles, setIsvalid } = props;
    const [errors, setErrors] = useState<any>({});

    const config: any = {
        maxSize: 1, // 1MB
        fileExt: props.ext,
        maxFiles: 1,
        accepts: props.accepts
    };

    const onFileChange = (event: any) => {
        setErrors({});

        let files = event.target.files;
        setFiles(files);

        isValidFiles(files);
    };

    const isValidFiles = (files: any) => {
        // Check Number of files
        if (files.length > config.maxFiles) {
            errors.errorText = `Error: At a time you can upload only ${config.maxFiles} file`;
            setErrors(errors);
            setIsvalid(false);
            return;
        }
        isValidFileExtension(files);
    };

    const isValidFileExtension = (files: any) => {
        // Make array of file extensions
        let extensions = config.fileExt.split(",").map(function (x: any) {
            return x.toLocaleUpperCase().trim();
        });

        for (let i = 0; i < files.length; i++) {
            // Get file extension
            let ext = files[i].name.toUpperCase().split(".").pop() || files[i].name;
            // Check the extension exists
            let exists = extensions.includes(ext);
            if (!exists) {
                errors.errorText = `Error: You can upload only .xlsx file`;
                setErrors(errors);
                setIsvalid(false);
                return;
            }
            // Check file size
            isValidFileSize(files[i]);
        }
    };

    const isValidFileSize = (file: any) => {
        let fileSizeinMB = file.size / (1024 * 1000);
        let size = Math.round(fileSizeinMB * 100) / 100; // convert upto 2 decimal place
        if (size > config.maxSize) {
            errors.errorText = `Error (File Size): ${file.name} exceed file size limit of ${config.maxSize} MB (${size} MB`;
            setIsvalid(false);
            return;
        }
        setIsvalid(true);
    };


    return <>
        <div className="upload-btn-wrapper w-500px text-center excel-upload bg-light p-2">
            <p className="step">
                {" "}
                <b>Drag and drop files</b> {" "}<br />
                or<br />
            </p>
            <i className="bi bi-cloud-upload-fill fs-1"></i>{" "}
            <br />
            <Form.Control className="input-drag"
                type="file"
                id="myfile"
                onChange={onFileChange}
                accept={props.accepts}
                //accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,.xlsx,.xls"
            />
            select file
        </div>
    </>
};

export default UploadLayout;

import React from 'react';
import { Link } from 'react-router-dom';
import { KTIcon } from '../_metronic/helpers';

interface CustomActionComponentProps {
    id: string;
    editPath: string;
    onDelete: (id: string) => void;
    onEditClick: (value: any) => void;
    isEditAllowed: boolean;
    isDeletedAllowed: boolean;
}

const CustomActionComponent: React.FC<CustomActionComponentProps> = ({ id, editPath, onDelete, onEditClick, isEditAllowed, isDeletedAllowed }) => {
    return (
        <div className="d-flex flex-row">
            {isEditAllowed && editPath && <Link className="btn btn-default mx-0 px-1" to={`${editPath}/${id}`}>
                <KTIcon iconName="pencil" iconType="outline" className="fs-4" />
            </Link>
            }
            {isEditAllowed && !editPath && <button className="btn btn-default mx-0 px-1" onClick={onEditClick}>
                <KTIcon iconName="pencil" iconType="outline" className="fs-4" />
            </button>
            }
            {isDeletedAllowed && <button className="btn btn-default mx-0 px-1" onClick={() => onDelete(id)}>
                <KTIcon iconName="trash" iconType="outline" className="text-danger fs-4" />
            </button>}
        </div>
    );
};

export default CustomActionComponent;

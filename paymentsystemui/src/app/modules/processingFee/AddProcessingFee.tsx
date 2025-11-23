import React, { useState } from 'react'
import ProcessingFeeForm from './partials/ProcessingFeeForm'
import { Content } from '../../../_metronic/layout/components/content'
import { PageLink, PageTitle } from '../../../_metronic/layout/core'
import { useParams } from 'react-router-dom'
import { KTCard, KTCardBody } from '../../../_metronic/helpers'
import { PageTitleWrapper } from '../../../_metronic/layout/components/toolbar/page-title'

const breadCrumbs: Array<PageLink> = [
    {
        title: 'Dashboard',
        path: '/dashboard',
        isSeparator: false,
        isActive: true,
    },
    {
        title: '',
        path: '',
        isSeparator: true,
        isActive: true,
    },
    {
        title: 'Additional Fee',
        path: '/processing-fee',
        isSeparator: false,
        isActive: true,
    },
    {
        title: '',
        path: '',
        isSeparator: true,
        isActive: true,
    },
];

const AddProcessingFee = () => {
    const { id } = useParams();
    const [title] = useState<any>(id == null ? "Add Additional Fee" : "Edit Additional Fee");

    return (
        <Content>
            <PageTitleWrapper />
            <PageTitle breadcrumbs={breadCrumbs}>{title}</PageTitle>
            <KTCard className='my-3'>
                <KTCardBody>
                    <ProcessingFeeForm />
                </KTCardBody>
            </KTCard>

        </Content>
    )
}

export default AddProcessingFee
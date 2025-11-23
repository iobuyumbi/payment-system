import React, { useEffect, useState } from 'react'
import { KTIcon, KTSVG, toAbsoluteUrl } from '../../_metronic/helpers'
import { Link } from 'react-router-dom';
import clsx from 'clsx';

export default function GetStarted(props: any) {
    const { icon, badgeColor } = props;
    const [steps, setSteps] = useState<any[]>([]);

    useEffect(() => {
        var step: any[] = [];
        step.push({ title: 'Setup projects', description: '', addLink: '/' });
        step.push({ title: 'Onboard farmers', description: '', addLink: '/' });
        step.push({ title: 'Setup Loan Products', description: '', addLink: '/' });
        step.push({ title: 'Import loan applications', description: '', addLink: '/' });

        setSteps(step);
    }, [])
    return (
        <div className='card border border-2 border-gray-300 border-hover'>
            {/* begin::Header */}
            <div className='card-header border-bottom pt-5'>
                <h3 className='card-title align-items-start flex-column'>
                    <span className='card-label fw-bold fs-3 mb-1'>Get Started</span>
                </h3>
            </div>
            {/* end::Header */}

            <div className='card-body p-9'>
                <div className="row">
                    <div className="col-md-4">
                        <h1 className='fs-1 text-gray-700'>
                            Welcome to Solidaridad
                        </h1>
                        <div className='fs-6 text-gray-700'>
                            Effectively manage farmer profiles, loan applications, payments and get useful insights for better decision making.
                        </div>
                    </div>
                    <div className="col-md-8">
                        {steps && steps.map((element: any, index: any) =>
                            <div className="mb-5  border-0" key={index}>
                                {/* begin::Item */}
                                <div className={clsx(
                                    'rounded  border border-dashed p-6',
                                    index === 0 ? 'bg-light-warning border-warning' : 'bg-light-warning border-warning',
                                )}>
                                    <div className='d-flex align-items-center'>

                                        {/* begin::Description */}
                                        <div className='flex-grow-1'>
                                            <a href='#' className='text-gray-900 text-hover-primary fw-bold fs-6'>
                                                {element.title}
                                            </a>
                                            <span className='text-gray-700 d-block'>{element.description}</span>
                                        </div>
                                        {/* end::Description */}
                                        {/* begin::Checkbox */}
                                        <KTIcon iconName='check-circle' className={clsx(
                                            'fs-1',
                                            index === 0 ? 'tick-success' : 'tick-success',
                                        )} />
                                        {/* end::Checkbox */}
                                    </div>
                                </div>

                                {/* end:Item */}
                            </div>
                        )}
                    </div>
                </div>

            </div>
        </div>
    )
}


import React from 'react'
import { KTCard, KTCardBody, toAbsoluteUrl } from '../../_metronic/helpers'

type Props = {
    className: string
}

const CountryWidget: React.FC<Props> = ({ className }) => {
    return (
        <>
            <KTCard className={className}>
                <KTCardBody>
                    <div className="d-flex align-items-center justify-content-between">
                        <div className='d-flex flex-center  '>
                            <div className='symbol symbol-50px symbol-circle'>
                                <img alt='Pic' src={toAbsoluteUrl('media/flags/kenya.svg')} />
                            </div>
                            <div className='px-5'>
                                <h1 className='fs-1 text-gray-900 fw-normal'>Kenya</h1>
                            </div>
                        </div>
                        <div>
                            <div className='d-flex flex-center flex-wrap '>
                                <div className='border border-gray-300 border-dashed rounded min-w-125px py-3 px-4 mx-3 mb-3'>
                                    <div className='fs-4 fw-bolder text-gray-700'>50</div>
                                    <div className=' text-gray-500'>Projects</div>
                                </div>

                                <div className='border border-gray-300 border-dashed rounded min-w-125px py-3 px-4 mx-3 mb-3'>
                                    <div className='fs-4 fw-bolder text-gray-700'>50,0000</div>
                                    <div className=' text-gray-500'>Farmers</div>
                                </div>

                                <div className='border border-gray-300 border-dashed rounded min-w-125px py-3 mx-3 px-4 mb-3'>
                                    <div className='fs-4 fw-bolder text-gray-700'>{'10,000'}</div>
                                    <div className=' text-gray-500'>Applications</div>
                                </div>
                            </div>
                        </div>
                        <div>
                            <button className='btn btn-sm btn-secondary'>Manage</button>
                        </div>
                    </div>
                </KTCardBody>
            </KTCard>

            <KTCard className={className}>
                <KTCardBody>
                    <div className="d-flex align-items-center justify-content-between">
                        <div className='d-flex flex-center  '>
                            <div className='symbol symbol-50px symbol-circle'>
                                <img alt='Pic' src={toAbsoluteUrl('media/flags/uganda.svg')} />
                            </div>
                            <div className='px-5'>
                                <h1 className='fs-1 text-gray-900 fw-normal'>Uganda</h1>
                            </div>
                        </div>
                        <div>
                            <div className='d-flex flex-center flex-wrap '>
                                <div className='border border-gray-300 border-dashed rounded min-w-125px py-3 px-4 mx-3 mb-3'>
                                    <div className='fs-4 fw-bolder text-gray-700'>50</div>
                                    <div className=' text-gray-500'>Projects</div>
                                </div>

                                <div className='border border-gray-300 border-dashed rounded min-w-125px py-3 px-4 mx-3 mb-3'>
                                    <div className='fs-4 fw-bolder text-gray-700'>50,0000</div>
                                    <div className=' text-gray-500'>Farmers</div>
                                </div>

                                <div className='border border-gray-300 border-dashed rounded min-w-125px py-3 mx-3 px-4 mb-3'>
                                    <div className='fs-4 fw-bolder text-gray-700'>{'10,000'}</div>
                                    <div className=' text-gray-500'>Applications</div>
                                </div>
                            </div>
                        </div>
                        <div>
                            <button className='btn btn-sm btn-secondary'>Manage</button>
                        </div>
                    </div>
                </KTCardBody>
            </KTCard>

            <KTCard className={className}>
                <KTCardBody>
                    <div className="d-flex align-items-center justify-content-between">
                        <div className='d-flex flex-center  '>
                            <div className='symbol symbol-50px symbol-circle'>
                                <img alt='Pic' src={toAbsoluteUrl('media/flags/tanzania.svg')} />
                            </div>
                            <div className='px-5'>
                                <h1 className='fs-1 text-gray-900 fw-normal'>Tanzania</h1>
                            </div>
                        </div>
                        <div>
                            <div className='d-flex flex-center flex-wrap '>
                                <div className='border border-gray-300 border-dashed rounded min-w-125px py-3 px-4 mx-3 mb-3'>
                                    <div className='fs-4 fw-bolder text-gray-700'>50</div>
                                    <div className=' text-gray-500'>Projects</div>
                                </div>

                                <div className='border border-gray-300 border-dashed rounded min-w-125px py-3 px-4 mx-3 mb-3'>
                                    <div className='fs-4 fw-bolder text-gray-700'>50,0000</div>
                                    <div className=' text-gray-500'>Farmers</div>
                                </div>

                                <div className='border border-gray-300 border-dashed rounded min-w-125px py-3 mx-3 px-4 mb-3'>
                                    <div className='fs-4 fw-bolder text-gray-700'>{'10,000'}</div>
                                    <div className=' text-gray-500'>Applications</div>
                                </div>
                            </div>
                        </div>
                        <div>
                            <button className='btn btn-sm btn-secondary'>Manage</button>
                        </div>
                    </div>
                </KTCardBody>
            </KTCard>

        </>
    )
}

export { CountryWidget }

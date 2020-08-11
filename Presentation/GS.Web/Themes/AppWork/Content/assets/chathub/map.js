(function (app) {
    app.controller("mapController", function ($scope) {
        var ContextMenu = document.getElementById("my-context-menu");
        var con = $.hubConnection("/signalr", { useDefaultPath: false });
        var hub = con.createHubProxy("chat");
        $scope.chucvuId = 0;
        $scope.thongBaoChucVuId = 0;
        $scope.keyword = "";

        con.start(function () {
            hub.invoke("GetAllChucVu");
            hub.invoke("GetAllDinhVi", $scope.keyword, "", $scope.chucvuId);
        });
        $scope.vitri = {};
        $scope.markers = [];
        $scope.centerMap = new google.maps.LatLng(21.020581, 105.8512163);
        $scope.mapOptions = {
            zoom: 14,
            center: $scope.centerMap,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            draggable: true,
            scaleControl: true,
            panControl: false
        };
        var map = document.getElementById('mapNhanVien');
        $scope.map = new google.maps.Map(map, $scope.mapOptions);
        var infoWindow = new google.maps.InfoWindow();

        hub.on("GetAllChucVu", function (chucvus) {
            $scope.$apply(function () {
                $scope.chucvus = JSON.parse(chucvus);
            });
        });

        hub.on("GetAllDinhVi", function (vitris) {
            $scope.$apply(function () {
                $scope.vitris = JSON.parse(vitris);
                $scope.initMarker();
            });
        });

        $scope.changeChucVu = function () {
            hub.invoke("GetAllDinhVi", $scope.keyword, "", $scope.chucvuId);
        }

        $scope.searchNhanVien = function () {
            hub.invoke("GetAllDinhVi", $scope.keyword, "", $scope.chucvuId);
        }

        $scope.clearAllMarker = function () {
            for (var i = 0; i < $scope.markers.length; i++) {
                $scope.markers[i].setMap(null);
            }
            $scope.markers = [];
        }

        $scope.initMarker = function () {
            $scope.clearAllMarker();
            var lastLocation = $scope.vitris[$scope.vitris.length - 1];
            $scope.centerMap = new google.maps.LatLng(lastLocation.Latitude, lastLocation.Longitude);
            $scope.map.setCenter($scope.centerMap);
            for (var i = 0; i < $scope.vitris.length; i++) {
                let vitri = $scope.vitris[i];
                let marker = new google.maps.Marker({
                    map: $scope.map,
                    position: new google.maps.LatLng(vitri.Latitude, vitri.Longitude),
                    title: vitri.Title,
                    icon: {
                        url: vitri.IconUrl,
                        scaledSize: new google.maps.Size(40, 40),
                        shape: { coords: [17, 17, 18], type: 'circle' },
                    }
                });
                marker.addListener('click', function (ev) {
                    HideContextMenuGoolge(ContextMenu);
                    var scale = Math.pow(2, $scope.map.getZoom());
                    var nw = new google.maps.LatLng(
                        $scope.map.getBounds().getNorthEast().lat(),
                        $scope.map.getBounds().getSouthWest().lng()
                    );
                    var worldCoordinateNW = $scope.map.getProjection().fromLatLngToPoint(nw);
                    var worldCoordinate = $scope.map.getProjection().fromLatLngToPoint(marker.getPosition());
                    var pixelOffset = new google.maps.Point(
                        Math.floor((worldCoordinate.x - worldCoordinateNW.x) * scale),
                        Math.floor((worldCoordinate.y - worldCoordinateNW.y) * scale)
                    );
                    $scope.vitri = vitri;
                    ShowContextMenuGoolge(ContextMenu, pixelOffset);
                });
                $scope.markers.push(marker);
            }
        }

        function HideContextMenuGoolge(ContextMenu) {
            ContextMenu.style.display = "none";
        }
        google.maps.event.addListener($scope.map, 'click', function () {
            HideContextMenuGoolge(ContextMenu);
        });

        function ShowContextMenuGoolge(ContextMenu, pixelOffset) {
            ContextMenu.style.top = (pixelOffset.y + 35) + "px";
            ContextMenu.style.left = (pixelOffset.x + 250) + "px";
            ContextMenu.style.display = "block";
        }

        $scope.chatMessage = function () {
            HideContextMenuGoolge(ContextMenu);
            $scope.$emit('chatMessageMap', $scope.vitri.nhanvien.ID);
        }

        $scope.pushNotification = function () {
            HideContextMenuGoolge(ContextMenu);
            $scope.titlePushNoti = '';
            $scope.contentPushNoti = '';
            $('#pushNotiModal').modal('show');
        }

        $scope.showHistoryWorking = function () {
            HideContextMenuGoolge(ContextMenu);
            $('#lichSuHanhTrinh').modal('show');
           
        }

        $scope.veLaiHanhTrinh = function () {
            $scope.txtNgayBatDau = $('#txtNgayBatDau').val();
            $scope.txtNgayKetThuc = $('#txtNgayKetThuc').val();
            if (!$scope.vitri.nhanvien) {
                alert('Bạn chưa chọn nhân viên');
                return;
            }

            if(!$scope.txtNgayBatDau || !$scope.txtNgayKetThuc) {
                alert('Chưa chọn ngày');
                return;
            }

            hub.invoke("VeLaiHanhTrinh", $scope.vitri.nhanvien.ID, $scope.txtNgayBatDau, $scope.txtNgayKetThuc);
            $('#lichSuHanhTrinh').modal('hide');
        }

        $scope.sendPushNoti = function () {
            if (!$scope.vitri.nhanvien) {
                alert('Bạn chưa chọn nhân viên');
                return;
            }

            hub.invoke("SendPushNoti", $scope.titlePushNoti, $scope.contentPushNoti, $scope.vitri.nhanvien.ID);
            $('#pushNotiModal').modal('hide');
        }

        var flightPath;
        var flightStart;
        var flightEnd;
        $scope.drawJourney = function (result) {
            if(!result || result.length == 0) {
                alert('Không có lịch sử hoạt động');
                return;
            }
            var checkKhoiTaoBanDo = 0;
            var arrInfor = result;
            var boundarydata = new Array();
            var boundaryStart = new Array();
            var boundaryEnd = new Array();

            for (inti = 0; inti < arrInfor.length; inti++) {
                var strAdd = new google.maps.LatLng(arrInfor[inti].Latitude, arrInfor[inti].Longitude);
                if (inti < 5) {
                    boundaryStart.push(strAdd);
                } else if(inti > (arrInfor.length - 5)) {
                    boundaryEnd.push(strAdd);
                } else {
                    boundarydata.push(strAdd);
                }
                
                
            }
            ArrDataPlayJourney = boundarydata;

            flightStart = new google.maps.Polyline({
                path: boundaryStart,
                geodesic: true,
                strokeColor: '#33CCFF',
                strokeOpacity: 1.0,
                strokeWeight: 2,
                icons: [{
                    icon: {
                        path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW,
                        strokeColor: '#00BCA4',
                        fillColor: '#00BCA4',
                        fillOpacity: 1,
                        scale: 3
                    },
                    repeat: '100px'
                }]
            });

            flightPath = new google.maps.Polyline({
                path: boundarydata,
                geodesic: true,
                strokeColor: '#33CCFF',
                strokeOpacity: 1.0,
                strokeWeight: 2,
                icons: [{
                    icon: {
                        path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW,
                        strokeColor: '#0000ff',
                        fillColor: '#0000ff',
                        fillOpacity: 1,
                        scale: 3
                    },
                    repeat: '100px'
                }]
            });

            flightEnd = new google.maps.Polyline({
                path: boundaryEnd,
                geodesic: true,
                strokeColor: '#33CCFF',
                strokeOpacity: 1.0,
                strokeWeight: 2,
                icons: [{
                    icon: {
                        path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW,
                        strokeColor: '#FF0000',
                        fillColor: '#FF0000',
                        fillOpacity: 1,
                        scale: 3
                    },
                    repeat: '100px'
                }]
            });
            if(arrInfor.length > 0) {
                $scope.centerMap = new google.maps.LatLng(arrInfor[0].Latitude, arrInfor[0].Longitude);
                $scope.map.setCenter($scope.centerMap);
            }
           
            flightStart.setMap($scope.map);
            flightPath.setMap($scope.map);
            flightEnd.setMap($scope.map);
        }

        $scope.removeLine = function () {
            if(flightStart != null) {
                flightStart.setMap(null);
            }

            if(flightPath != null) {
                flightPath.setMap(null);
            }

            if(flightEnd != null) {
                flightEnd.setMap(null);
            }
        }

        hub.on("VeLaiHanhTrinh", function (hanhtrinhs) {
            $scope.$apply(function () {
                $scope.removeLine();
                $scope.drawJourney(hanhtrinhs);
            });
        });

        $scope.openThongBaoTheoChucVu = function () {
            $('#guiThongBaoTheoChucVu').modal('show');
        }

        $scope.guiThongBaoTheoChucVu = function () {
            hub.invoke("GuiThongBaoTheoChucVu", $scope.thongBaoChucVuId, $scope.titlePushNotiChucVu, $scope.contentPushNotiChucVu);
            $scope.thongBaoChucVuId = 0;
            $scope.titlePushNotiChucVu = '';
            $scope.contentPushNotiChucVu = '';
            $('#guiThongBaoTheoChucVu').modal('hide');
        }

        $("#page-map").css("display", "block");
    });
})(angular.module('app'));
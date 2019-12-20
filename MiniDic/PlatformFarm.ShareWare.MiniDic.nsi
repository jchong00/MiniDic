!include "Library.nsh"
!include "Sections.nsh"

; C:\Program Files (x86)\NSIS\Unicode\Include
; C:\Program Files (x86)\NSIS\Unicode\Plugins

Unicode true

!define PRODUCT_NAME            "PlatformFarm MiniDic"
!define PRODUCT_VERSION         "1,0,0,0"
!define PRODUCT_PUBLISHER       "jchong00@gmail.com"
!define PRODUCT_WEB_SITE        "https://rightnowdo.tistory.com"
!define PRODUCT_UNINST_KEY      "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"
!define PRODUCT_INSTALL_PATH "$PROGRAMFILES\PlatformFarm\MiniDic"
!define INSTALL_FILES_PATH   ".\bin\Release"

Name             "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile          ".\out\PlatformFarm.ShareWare.MiniDic.Setup.exe"
#LoadLanguageFile "${NSISDIR}\Contrib\Language files\Korean.nlf"
#LoadLanguageFile "${NSISDIR}\Contrib\Language files\Japanese.nlf"
InstallDir       "${PRODUCT_INSTALL_PATH}"
Icon             "${NSISDIR}\Contrib\Graphics\Icons\orange-install.ico"
UninstallIcon    "${NSISDIR}\Contrib\Graphics\Icons\orange-uninstall.ico"

ShowInstDetails show
ShowUnInstDetails show



;-----------------------------------------------------------------------------------------------------------------------
RequestExecutionLevel admin

Function .onInit
 
  ReadRegStr $R0 HKLM \
  "${PRODUCT_UNINST_KEY}" \
  "UninstallString"
  StrCmp $R0 "" done
 
  ;"${PRODUCT_NAME} is already installed. $\n$\nClick `OK` to remove the \
  ;previous version or `Cancel` to cancel this upgrade." \

  MessageBox MB_OKCANCEL|MB_ICONEXCLAMATION \
  "${PRODUCT_NAME}은 이미 설치되어 있습니다. $\n$\n`OK`를 클릭하면 \
  업그레이드를 진행 합니다. `Cancel`을 클릭하면 설치가 취소 됩니다." \
  IDOK uninst
  Abort
 
;Run the uninstaller
uninst:
  ClearErrors
  ExecWait '$R0 _?=$INSTDIR' ;Do not copy the uninstaller to a temp file
 
  IfErrors no_remove_uninstaller done
    ;You can either use Delete /REBOOTOK in the uninstaller or add some code
    ;here to remove the uninstaller. Use a registry key to check
    ;whether the user has chosen to uninstall. If you are using an uninstaller
    ;components page, make sure all sections are uninstalled.
  no_remove_uninstaller:
 
done:
 
FunctionEnd
  
Section "PlatformFarm MiniDic Install" SEC01

  ReadRegStr $0 HKLM "SOFTWARE\Microsoft\Windows NT\CurrentVersion" "ProductName"

  ExecWait "TaskKill /F /IM PlatformFarm.ShareWare.MiniDic.exe"
      
  SetOutPath "${PRODUCT_INSTALL_PATH}"
  File "${INSTALL_FILES_PATH}\Resources\MiniDic.ico"
  File "${INSTALL_FILES_PATH}\cef.pak"
  File "${INSTALL_FILES_PATH}\CefSharp.BrowserSubprocess.Core.dll"
  File "${INSTALL_FILES_PATH}\CefSharp.BrowserSubprocess.exe"
  File "${INSTALL_FILES_PATH}\CefSharp.Core.dll"
  File "${INSTALL_FILES_PATH}\CefSharp.Core.xml"
  File "${INSTALL_FILES_PATH}\CefSharp.dll"
  File "${INSTALL_FILES_PATH}\CefSharp.WinForms.dll"
  File "${INSTALL_FILES_PATH}\CefSharp.WinForms.XML"
  File "${INSTALL_FILES_PATH}\CefSharp.XML"
  File "${INSTALL_FILES_PATH}\cef_100_percent.pak"
  File "${INSTALL_FILES_PATH}\cef_200_percent.pak"
  File "${INSTALL_FILES_PATH}\cef_extensions.pak"
  File "${INSTALL_FILES_PATH}\chrome_elf.dll"
  File "${INSTALL_FILES_PATH}\d3dcompiler_47.dll"
  File "${INSTALL_FILES_PATH}\devtools_resources.pak"
  File "${INSTALL_FILES_PATH}\icudtl.dat"
  File "${INSTALL_FILES_PATH}\libcef.dll"
  File "${INSTALL_FILES_PATH}\libEGL.dll"
  File "${INSTALL_FILES_PATH}\libGLESv2.dll"
  File "${INSTALL_FILES_PATH}\natives_blob.bin"
  File "${INSTALL_FILES_PATH}\PlatformFarm.ShareWare.MiniDic.exe"
  File "${INSTALL_FILES_PATH}\PlatformFarm.ShareWare.MiniDic.exe.config"
  File "${INSTALL_FILES_PATH}\README.txt"
  File "${INSTALL_FILES_PATH}\snapshot_blob.bin"
  File "${INSTALL_FILES_PATH}\v8_context_snapshot.bin"
  SetOutPath "${PRODUCT_INSTALL_PATH}\locales"
  File "${INSTALL_FILES_PATH}\locales\*.pak"
  SetOutPath "${PRODUCT_INSTALL_PATH}\swiftshader"
  File "${INSTALL_FILES_PATH}\swiftshader\libEGL.dll"
  File "${INSTALL_FILES_PATH}\swiftshader\libGLESv2.dll"

  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Run" "MiniDic" "$INSTDIR\PlatformFarm.ShareWare.MiniDic.exe"
  
  SetAutoClose true

SectionEnd

Section -AdditionalIcons
  ; At Desktop window
  CreateShortCut "$DESKTOP\MiniDic.lnk" "$INSTDIR\PlatformFarm.ShareWare.MiniDic.exe" "PlatformFarm Mini Dictionary" "$INSTDIR\MiniDic.ico" 0 SW_SHOWNORMAL
SectionEnd

Section -Post

  WriteUninstaller "$INSTDIR\Uninstall.PlatformFarm.ShareWare.MiniDic.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\Uninstall.PlatformFarm.ShareWare.MiniDic.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
  
SectionEnd


Section Uninstall

  ExecWait "TaskKill /F /IM PlatformFarm.ShareWare.MiniDic.exe"
  
  Delete "$DESKTOP\MiniDic.lnk"

  RMDir /r "${PRODUCT_INSTALL_PATH}"  

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"      

  SetAutoClose true
  
SectionEnd
